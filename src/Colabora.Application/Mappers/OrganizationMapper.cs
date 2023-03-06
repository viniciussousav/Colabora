using Colabora.Application.Features.GetOrganizationById;
using Colabora.Application.Features.GetOrganizationById.Models;
using Colabora.Application.Features.RegisterOrganization.Models;
using Colabora.Domain.Entities;

namespace Colabora.Application.Mappers;

public static class OrganizationMapper
{
    public static RegisterOrganizationResponse MapToRegisterOrganizationResponse(this Organization organization)
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
    
    public static GetOrganizationByIdResponse MapToGetOrganizationByIdResponse(this Organization organization)
    {
        return new GetOrganizationByIdResponse(
            OrganizationId: organization.OrganizationId,
            Name: organization.Name,
            State: organization.State,
            Interests: organization.Interests,
            CreatedBy: organization.CreatedBy,
            CreatedAt: organization.CreatedAt,
            SocialActions: organization.SocialActions.Select(action => 
                new OrganizationSocialActionDetails(
                    SocialActionId: action.SocialActionId,
                    SocialActionTitle: action.Title,
                    CreatedAt: action.CreatedAt,
                    OccurrenceDate: action.OccurrenceDate)).ToList());
    }
}