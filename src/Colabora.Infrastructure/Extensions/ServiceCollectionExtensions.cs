using System.Diagnostics.CodeAnalysis;
using Colabora.Domain.Organization;
using Colabora.Domain.SocialAction;
using Colabora.Domain.Volunteer;
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
        services
            .AddDbContext<AppDbContext>(options =>
            {
                var sqlConnectionString = configuration.GetValue<string>("SQL_COLABORA_DATABASE");
                options.UseNpgsql(sqlConnectionString);
            })
            .AddHealthChecks()
            .AddDbContextCheck<AppDbContext>();
    }

    private static void AddRepositories(this IServiceCollection services)
    {
        services.AddTransient<IOrganizationRepository, OrganizationRepository>();
        services.AddTransient<IVolunteerRepository, VolunteerRepository>();
        services.AddTransient<ISocialActionRepository, SocialActionRepository>();
    }

    private static void AddServices(this IServiceCollection services)
    {
        services.AddTransient<IGoogleAuthProvider, GoogleAuthProvider>();
        services.AddTransient<IAuthService, AuthService>();
        services.AddTransient<IMessageProducer, MessageProducer>();
        services.AddTransient<IEmailSender, EmailSender>();
    }
    
    public static void AddInfrastructure(this IServiceCollection services, IConfiguration configuration)
    {
        services.AddDatabasePersistence(configuration);
        services.AddRepositories();
        services.AddAwsServices();
        services.AddServices();
    }
}