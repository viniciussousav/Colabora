using System.Diagnostics.CodeAnalysis;
using Colabora.Domain.Repositories;
using Colabora.Infrastructure.Auth;
using Colabora.Infrastructure.Auth.Google;
using Colabora.Infrastructure.Messaging.Producer;
using Colabora.Infrastructure.Persistence;
using Colabora.Infrastructure.Persistence.Repositories;
using Colabora.Infrastructure.Services.EmailSender;
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
            options.UseNpgsql(sqlConnectionString);
        });
    }

    private static void AddRepositories(this IServiceCollection services)
    {
        services.AddScoped<IOrganizationRepository, OrganizationRepository>();
        services.AddScoped<IVolunteerRepository, VolunteerRepository>();
        services.AddScoped<ISocialActionRepository, SocialActionRepository>();
    }

    private static void AddServices(this IServiceCollection services)
    {
        // Auth
        services.AddScoped<IGoogleAuthService, GoogleAuthService>();
        services.AddScoped<IAuthService, AuthService>();

        services.AddScoped<IMessageProducer, MessageProducer>();
        services.AddScoped<IEmailSender, EmailSender>();
    }
    
    public static void AddInfrastructure(this IServiceCollection services, IConfiguration configuration)
    {
        services.AddDatabasePersistence(configuration);
        services.AddRepositories();
        services.AddAmazonSqsConfiguration(configuration);
        services.AddServices();
    }
}