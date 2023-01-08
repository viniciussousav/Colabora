using System.Diagnostics.CodeAnalysis;
using Colabora.Application.UseCases.GetVolunteers.Models;
using Colabora.Application.UseCases.RegisterOrganization.Models;
using Colabora.Application.UseCases.RegisterVolunteer.Models;
using MediatR;
using Microsoft.Extensions.DependencyInjection;

namespace Colabora.Application.Shared;

[ExcludeFromCodeCoverage]
public static class ServiceCollectionExtensions
{
    private static void AddMediatrAssemblies(this IServiceCollection services)
    {
        services.AddMediatR(typeof(GetVolunteersResponse));
        services.AddMediatR(typeof(RegisterOrganizationCommand));
        services.AddMediatR(typeof(RegisterVolunteerCommand));
    }

    public static void AddApplicationDependencies(this IServiceCollection services)
    {
        services.AddMediatrAssemblies();
    }
}