using System.Diagnostics.CodeAnalysis;
using Colabora.Domain.Organization;
using Colabora.Domain.SocialAction;
using Colabora.Domain.Volunteer;
using Colabora.Infrastructure.Auth;
using Colabora.Infrastructure.Auth.Google;
using Colabora.Infrastructure.Messaging.Producer;
using Colabora.Infrastructure.Persistence.DynamoDb.Repositories.EmailVerification;
using Colabora.Infrastructure.Persistence.SqlServer;
using Colabora.Infrastructure.Persistence.SqlServer.Repositories;
using Colabora.Infrastructure.Services.EmailSender;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.DependencyInjection;

namespace Colabora.Infrastructure.Extensions;

[ExcludeFromCodeCoverage]
public static class ServiceCollectionExtensions
{
    private static void AddDatabasePersistence(this IServiceCollection services)
    {
        services
            .AddDbContext<AppDbContext>(options =>
            {
                var sqlConnectionString = Environment.GetEnvironmentVariable("ColaboraSqlDatabase");
                ArgumentNullException.ThrowIfNull(sqlConnectionString, nameof(sqlConnectionString));
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
        services.AddTransient<IEmailVerificationRepository, EmailVerificationRepository>();
    }

    private static void AddServices(this IServiceCollection services)
    {
        // Auth
        services.AddTransient<IGoogleAuthProvider, GoogleAuthProvider>();
        services.AddTransient<IAuthService, AuthService>();

        services.AddTransient<IMessageProducer, MessageProducer>();
        services.AddTransient<IEmailSender, EmailSender>();
    }
    
    public static void AddInfrastructure(this IServiceCollection services)
    {
        services.AddDatabasePersistence();
        services.AddRepositories();
        services.AddAwsServices();
        services.AddServices();
    }
}