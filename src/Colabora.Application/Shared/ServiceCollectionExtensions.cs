using System.Diagnostics.CodeAnalysis;
using Colabora.Application.Features.Organization.GetOrganizationById.Models;
using Colabora.Application.Features.Volunteer.GetVolunteerById.Models;
using Colabora.Application.Features.Volunteer.GetVolunteers.Models;
using Colabora.Application.Features.Volunteer.RegisterOrganization.Models;
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
        services.AddMediatR(typeof(GetVolunteerByIdQuery));
        services.AddMediatR(typeof(GetVolunteersQuery));
        services.AddMediatR(typeof(RegisterVolunteerCommand));

        services.AddMediatR(typeof(GetOrganizationByIdQuery));
        services.AddMediatR(typeof(RegisterOrganizationCommand));
    }

    private static void AddServices(this IServiceCollection services)
    {
        services.AddTransient<IGoogleAuthProvider, GoogleAuthProvider>();
    }

    private static void AddValidators(this IServiceCollection services)
    {
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