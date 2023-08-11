using System;
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
            .AddEnvironmentVariables()
            .Build();
    }
    
    private AppDbContext CreateContext()
    {
        var connectionString = _configuration.GetValue<string>("SQL_COLABORA_DATABASE");
        return new AppDbContext(
            new DbContextOptionsBuilder<AppDbContext>()
                .UseNpgsql(connectionString)
                .Options);
    }
    
    public async Task ResetDatabase()
    {
        var ctx = CreateContext();
        await ctx.Database.EnsureDeletedAsync();
        await ctx.Database.EnsureCreatedAsync();
        await ctx.DisposeAsync();
    }
}