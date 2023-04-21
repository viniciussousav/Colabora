using System.Diagnostics.CodeAnalysis;
using Colabora.Application.Features.Organization.RegisterOrganization.Models;
using Colabora.Application.Features.SocialAction.CreateSocialAction.Models;
using Colabora.Application.Features.SocialAction.JoinSocialAction.Models;
using Colabora.Application.Features.Volunteer.GetVolunteers.Models;
using Colabora.Application.Features.Volunteer.RegisterVolunteer.Models;
using Colabora.Application.Validation;
using Colabora.Infrastructure.Auth.Google;
using FluentValidation;
using MediatR;
using Microsoft.Extensions.DependencyInjection;

namespace Colabora.Application.Shared;

[ExcludeFromCodeCoverage]
public static class ServiceCollectionExtensions
{
    private static void AddMediatrAssemblies(this IServiceCollection services)
    {
        services.AddMediatR(typeof(GetVolunteersQuery));
        services.AddMediatR(typeof(RegisterOrganizationCommand));
        services.AddMediatR(typeof(RegisterVolunteerCommand));
        services.AddMediatR(typeof(CreateSocialActionCommand));
    }

    private static void AddServices(this IServiceCollection services)
    {
        services.AddScoped<IGoogleAuthService, GoogleAuthService>();
    }

    private static void AddValidators(this IServiceCollection services)
    {
        services.AddSingleton<IValidator<CreateSocialActionCommand>, CreateSocialActionValidator>();
        services.AddSingleton<IValidator<JoinSocialActionCommand>, JoinSocialActionValidator>();
        services.AddSingleton<IValidator<RegisterOrganizationCommand>, RegisterOrganizationValidator>();
        services.AddSingleton<IValidator<RegisterVolunteerCommand>, RegisterVolunteerValidator>();
    }
    
    public static void AddApplicationDependencies(this IServiceCollection services)
    {
        services.AddMediatrAssemblies();
        services.AddServices();
        services.AddValidators();
    }
}