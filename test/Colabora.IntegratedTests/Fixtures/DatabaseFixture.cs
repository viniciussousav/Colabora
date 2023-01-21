using System;
using System.Threading.Tasks;
using Colabora.Infrastructure.Persistence;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;

namespace Colabora.IntegrationTests.Fixtures;

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

    private static async Task ClearDatabase()
    {
        try
        {
            var appDbContext = CreateContext();
        
            appDbContext.RemoveRange(appDbContext.Volunteers);
            appDbContext.RemoveRange(appDbContext.Organizations);
            appDbContext.RemoveRange(appDbContext.SocialActions);

            await appDbContext.SaveChangesAsync();
            await appDbContext.DisposeAsync();
        }
        catch (Exception)
        {
            // Ignore if database is already empty
        }
    }
    
    private static async Task ApplyMigration()
    {
        try
        {
            var appDbContext = CreateContext();
            await appDbContext.Database.MigrateAsync();
            await appDbContext.DisposeAsync();
        }
        catch (Exception)
        {
            // Ignore if database already exists
        }
    }

    public static async Task ResetDatabase()
    {
        await ApplyMigration();
        await ClearDatabase();
    }
}