using System.Diagnostics.CodeAnalysis;
using Colabora.Domain.Repositories;
using Colabora.Infrastructure.Persistence.Repositories;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;

namespace Colabora.Infrastructure.Persistence.Extensions;

[ExcludeFromCodeCoverage]
public static class ServiceCollectionExtensions
{
    private static void AddDatabasePersistence(this IServiceCollection services, IConfiguration configuration)
    {
        services.AddDbContextFactory<AppDbContext>(options =>
        {
            var sqlConnectionString = configuration.GetConnectionString("ColaboraDatabase");
            options.UseSqlServer(sqlConnectionString);
        }, ServiceLifetime.Scoped);
    }

    private static void AddRepositories(this IServiceCollection services)
    {
        services.AddScoped<IOrganizationRepository, OrganizationRepository>();
        services.AddScoped<IVolunteerRepository, VolunteerRepository>();
        services.AddScoped<ISocialActionRepository, SocialActionRepository>();
    }

    public static void AddInfrastructure(this IServiceCollection services, IConfiguration configuration)
    {
        services.AddDatabasePersistence(configuration);
        services.AddRepositories();
    }
}