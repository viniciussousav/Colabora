using Colabora.Application.Features.Organization.GetOrganizationById.Models;
using Colabora.Application.Features.Volunteer.RegisterOrganization.Models;
using Colabora.Domain.Organization;

namespace Colabora.Application.Mappers;

public static class OrganizationMapper
{
    public static RegisterOrganizationResponse MapToRegisterOrganizationResponse(this Organization organization)
    {
        return new RegisterOrganizationResponse(
            OrganizationId: organization.Id,
            Name: organization.Name,
            Email: organization.Email,
            State: organization.State,
            Interests: organization.Interests,
            VolunteerCreatorId: organization.VolunteerCreatorId,
            CreatedAt: organization.CreatedAt.ToUniversalTime());
    }
    
    public static Organization MapToOrganization(this RegisterOrganizationCommand organization)
    {
        return new Organization(
            name: organization.Name,
            email: organization.Email,
            state: organization.State,
            interests: organization.Interests,
            volunteerCreatorId: organization.VolunteerCreatorId);
    }
    
    public static GetOrganizationByIdResponse MapToGetOrganizationByIdResponse(this Organization organization)
    {
        return new GetOrganizationByIdResponse(
            OrganizationId: organization.Id,
            Name: organization.Name,
            Email: organization.Email,
            State: organization.State,
            Interests: organization.Interests,
            VolunteerCreatorId: organization.VolunteerCreatorId,
            CreatedAt: organization.CreatedAt.ToUniversalTime());
    }
}