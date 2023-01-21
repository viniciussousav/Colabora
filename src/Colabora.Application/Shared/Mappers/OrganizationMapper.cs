using Colabora.Application.UseCases.RegisterOrganization.Models;
using Colabora.Domain.Entities;

namespace Colabora.Application.Shared.Mappers;

public static class OrganizationMapper
{
    public static RegisterOrganizationResponse MapToResponse(this Organization organization)
    {
        return new RegisterOrganizationResponse(
            OrganizationId: organization.OrganizationId,
            Name: organization.Name,
            State: organization.State,
            Interests: organization.Interests,
            CreatedBy: organization.CreatedBy,
            CreatedAt: organization.CreatedAt);
    }
    
    public static Organization MapToOrganization(this RegisterOrganizationCommand organization)
    {
        return new Organization(
            name: organization.Name,
            email: organization.Email,
            state: organization.State,
            interests: organization.Interests,
            createdBy: organization.VolunteerCreatorId);
    }
}