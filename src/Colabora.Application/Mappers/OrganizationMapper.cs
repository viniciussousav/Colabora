using Colabora.Application.Features.Organization.GetOrganizationById.Models;
using Colabora.Application.Features.Organization.RegisterOrganization.Models;
using Colabora.Domain.Organization;
using Colabora.Infrastructure.Persistence.DynamoDb.Repositories.EmailVerification.Models;

namespace Colabora.Application.Mappers;

public static class OrganizationMapper
{
    public static RegisterOrganizationResponse MapToRegisterOrganizationResponse(this Organization organization)
    {
        return new RegisterOrganizationResponse(
            OrganizationId: organization.OrganizationId,
            Name: organization.Name,
            Email: organization.Email,
            State: organization.State,
            Interests: organization.Interests,
            CreatedBy: organization.CreatedBy,
            CreatedAt: organization.CreatedAt.ToUniversalTime(),
            Verified: organization.Verified);
    }
    
    public static Organization MapToOrganization(this RegisterOrganizationCommand organization)
    {
        return new Organization(
            name: organization.Name,
            email: organization.Email,
            state: organization.State,
            interests: organization.Interests,
            createdBy: organization.VolunteerCreatorId,
            verified: false);
    }
    
    public static GetOrganizationByIdResponse MapToGetOrganizationByIdResponse(this Organization organization)
    {
        return new GetOrganizationByIdResponse(
            OrganizationId: organization.OrganizationId,
            Name: organization.Name,
            State: organization.State,
            Interests: organization.Interests,
            CreatedBy: organization.CreatedBy,
            CreatedAt: organization.CreatedAt.ToUniversalTime(),
            SocialActions: organization.SocialActions.Select(action => 
                new OrganizationSocialActionDetails(
                    SocialActionId: action.SocialActionId,
                    SocialActionTitle: action.Title,
                    CreatedAt: action.CreatedAt.ToUniversalTime(),
                    OccurrenceDate: action.OccurrenceDate)).ToList());
    }

    public static EmailVerificationRequest MapToEmailVerificationRequest(this RegisterOrganizationResponse response)
    {
        return new EmailVerificationRequest
        {
            Email = response.Email,
            Code = Guid.NewGuid()
        };
    }
}