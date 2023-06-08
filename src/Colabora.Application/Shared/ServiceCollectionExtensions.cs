using System.Diagnostics.CodeAnalysis;
using Colabora.Application.Features.Organization.GetOrganizationById.Models;
using Colabora.Application.Features.Organization.RegisterOrganization.Models;
using Colabora.Application.Features.Organization.VerifyOrganization.Models;
using Colabora.Application.Features.SocialAction.CreateSocialAction.Models;
using Colabora.Application.Features.SocialAction.GetSocialActionById.Models;
using Colabora.Application.Features.SocialAction.GetSocialActions.Models;
using Colabora.Application.Features.SocialAction.JoinSocialAction.Models;
using Colabora.Application.Features.Volunteer.GetVolunteerById.Models;
using Colabora.Application.Features.Volunteer.GetVolunteers.Models;
using Colabora.Application.Features.Volunteer.RegisterVolunteer.Models;
using Colabora.Application.Services.EmailVerification;
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
        services.AddMediatR(typeof(GetVolunteerByIdQuery));
        services.AddMediatR(typeof(GetVolunteersQuery));
        services.AddMediatR(typeof(RegisterVolunteerCommand));

        services.AddMediatR(typeof(GetOrganizationByIdQuery));
        services.AddMediatR(typeof(RegisterOrganizationCommand));
        services.AddMediatR(typeof(VerifyOrganizationCommand));

        services.AddMediatR(typeof(CreateSocialActionCommand));
        services.AddMediatR(typeof(GetSocialActionByIdQuery));
        services.AddMediatR(typeof(GetSocialActionsQuery));
        services.AddMediatR(typeof(JoinSocialActionCommand));
    }

    private static void AddServices(this IServiceCollection services)
    {
        services.AddTransient<IGoogleAuthService, GoogleAuthService>();
        services.AddTransient<IEmailVerificationService, EmailVerificationService>();
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