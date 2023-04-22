using System.Diagnostics.CodeAnalysis;
using Colabora.Domain.Repositories;
using Colabora.Infrastructure.Auth;
using Colabora.Infrastructure.Auth.Google;
using Colabora.Infrastructure.Persistence;
using Colabora.Infrastructure.Persistence.Repositories;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;

namespace Colabora.Infrastructure.Extensions;

[ExcludeFromCodeCoverage]
public static class ServiceCollectionExtensions
{
    private static void AddDatabasePersistence(this IServiceCollection services, IConfiguration configuration)
    {
        services.AddDbContext<AppDbContext>(options =>
        {
            var sqlConnectionString = configuration.GetConnectionString("ColaboraDatabase");
            options.UseSqlServer(sqlConnectionString);
        });
    }

    private static void AddRepositories(this IServiceCollection services)
    {
        services.AddScoped<IOrganizationRepository, OrganizationRepository>();
        services.AddScoped<IVolunteerRepository, VolunteerRepository>();
        services.AddScoped<ISocialActionRepository, SocialActionRepository>();
    }

    private static void AddAuthServices(this IServiceCollection services, IConfiguration configuration)
    {
        services.AddScoped<IGoogleAuthService, GoogleAuthService>();
        services.AddScoped<IAuthService, AuthService>();
    }
    
    public static void AddInfrastructure(this IServiceCollection services, IConfiguration configuration)
    {
        services.AddDatabasePersistence(configuration);
        services.AddRepositories();
        services.AddAuthServices(configuration);
    }
}