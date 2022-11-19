using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;

namespace Colabora.Infrastructure.Persistence.Configuration;

public static class ServiceCollectionExtensions
{
    public static IServiceCollection AddDatabases(this IServiceCollection services, IConfiguration configuration)
    {
        services.AddDbContext<AppDbContext>(options =>
        {
            var sqlConnectionString = configuration.GetConnectionString("ColaboraDatabase");
            options.UseSqlServer(sqlConnectionString);
        });

        return services;
    }
}