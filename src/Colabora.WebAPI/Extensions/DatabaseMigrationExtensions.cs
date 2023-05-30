﻿using Colabora.Infrastructure.Persistence.SqlServer;

namespace Colabora.WebAPI.Extensions;

public static class DatabaseMigrationExtensions
{
    public static void CreateDatabase(this WebApplication app)
    {
        using var scope = app.Services.CreateScope();
        var db = scope.ServiceProvider.GetRequiredService<AppDbContext>();
        db.Database.EnsureDeleted();
        db.Database.EnsureCreated();
    }
}