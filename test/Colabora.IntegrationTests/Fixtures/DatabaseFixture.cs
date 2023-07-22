using System;
using System.Threading.Tasks;
using Colabora.Infrastructure.Persistence.SqlServer;
using Microsoft.EntityFrameworkCore;

// ReSharper disable ClassNeverInstantiated.Global

namespace Colabora.IntegrationTests.Fixtures;

public class DatabaseFixture
{
    private AppDbContext CreateContext()
    {
        var connectionString = Environment.GetEnvironmentVariable("SQL_COLABORA_DATABASE");
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