using System.Threading.Tasks;
using Colabora.Infrastructure.Persistence;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;

namespace Colabora.IntegrationTests.Fixtures;

public class DatabaseFixture
{
    private readonly AppDbContext _appDbContext;
    public DatabaseFixture()
    {
        _appDbContext = CreateContext();
    }
    
    private static IConfiguration GetConfiguration() =>
        new ConfigurationBuilder()
            .AddJsonFile("appsettings.Test.json")
            .Build();
    
    private static AppDbContext CreateContext()
    {
        var connectionString = GetConfiguration().GetConnectionString("ColaboraDatabase");
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