using System.Threading.Tasks;
using Colabora.Infrastructure.Persistence;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;

// ReSharper disable ClassNeverInstantiated.Global

namespace Colabora.IntegrationTests.Fixtures;

public class DatabaseFixture
{
    private readonly IConfigurationRoot _configuration;
    
    public DatabaseFixture()
    {
        _configuration = new ConfigurationBuilder()
            .AddJsonFile("appsettings.Test.json")
            .Build();
        
    }
    
    private AppDbContext CreateContext()
    {
        var connectionString = _configuration.GetConnectionString("ColaboraDatabase");
        return new AppDbContext(
            new DbContextOptionsBuilder<AppDbContext>()
                .UseSqlServer(connectionString)
                .Options);
    }
    
    public async Task ResetDatabase()
    {
        var ctx = CreateContext();
        await ctx.Database.EnsureDeletedAsync();
        await ctx.DisposeAsync();
    }
}