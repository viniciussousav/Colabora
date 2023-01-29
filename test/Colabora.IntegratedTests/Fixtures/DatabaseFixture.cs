using System.Threading.Tasks;
using Colabora.Infrastructure.Persistence;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;

// ReSharper disable ClassNeverInstantiated.Global

namespace Colabora.IntegrationTests.Fixtures;

public class DatabaseFixture
{
    private readonly AppDbContext _appDbContext;
    private readonly IConfigurationRoot _configuration;
    public DatabaseFixture()
    {
        _configuration = new ConfigurationBuilder()
            .AddJsonFile("appsettings.Test.json")
            .Build();
        
        _appDbContext = CreateContext();
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
        await _appDbContext.Database.EnsureDeletedAsync();
    }
}