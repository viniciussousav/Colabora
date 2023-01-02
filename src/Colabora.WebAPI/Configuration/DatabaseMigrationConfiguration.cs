using Colabora.Infrastructure.Persistence;
using Microsoft.EntityFrameworkCore;

namespace Colabora.WebAPI.Configuration;

public static class DatabaseMigrationConfiguration
{
    public static void ApplyMigration(this WebApplication app)
    {
        using var scope = app.Services.CreateScope();
        var service = scope.ServiceProvider.GetRequiredService<AppDbContext>();

        service.Database.MigrateAsync();
    }
}