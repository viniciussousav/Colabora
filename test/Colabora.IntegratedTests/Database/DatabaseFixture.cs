using System.Threading.Tasks;
using Colabora.Infrastructure.Persistence;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;

namespace Colabora.IntegrationTests.Database;

public static class DatabaseFixture
{
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

    public static async Task ClearDatabase()
    {
        var appDbContext = CreateContext();
        
        appDbContext.RemoveRange(appDbContext.Volunteers);
        appDbContext.RemoveRange(appDbContext.Organizations);
        
        await appDbContext.SaveChangesAsync();
        await appDbContext.DisposeAsync();
    }
}