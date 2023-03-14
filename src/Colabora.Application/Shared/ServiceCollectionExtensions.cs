using System.Diagnostics.CodeAnalysis;
using Colabora.Application.Features.Organization.RegisterOrganization.Models;
using Colabora.Application.Features.SocialAction.CreateSocialAction.Models;
using Colabora.Application.Features.Volunteer.GetVolunteers.Models;
using Colabora.Application.Features.Volunteer.RegisterVolunteer.Models;
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

    public static void AddApplicationDependencies(this IServiceCollection services)
    {
        services.AddMediatrAssemblies();
    }
}