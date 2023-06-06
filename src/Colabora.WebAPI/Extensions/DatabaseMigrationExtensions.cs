using Colabora.Infrastructure.Persistence.SqlServer;
using Microsoft.EntityFrameworkCore;

namespace Colabora.WebAPI.Extensions;

public static class DatabaseMigrationExtensions
{
    public static void CreateDatabase(this WebApplication app)
    {
        using var scope = app.Services.CreateScope();
        var db = scope.ServiceProvider.GetRequiredService<AppDbContext>();
        db.Database.Migrate();
    }
}