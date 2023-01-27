using Colabora.Application.UseCases.CreateSocialAction.Models;
using Colabora.Application.UseCases.GetSocialActions.Models;
using Colabora.Domain.Entities;

namespace Colabora.Application.Shared.Mappers;

public static class SocialActionMapper
{
    public static SocialAction MapToSocialAction(this CreateSocialActionCommand command)
        => new (
            title: command.Title,
            description: command.Description,
            organizationId: command.OrganizationId,
            volunteerCreatorId: command.VolunteerCreatorId,
            state: command.State,
            interests: command.Interests,
            occurrenceDate: command.OccurrenceDate,
            createdAt: DateTimeOffset.Now);
    
    public static CreateSocialActionResponse MapToCreateSocialActionResponse(this SocialAction socialAction)
        => new (
            SocialActionId: socialAction.SocialActionId,
            Title: socialAction.Title,
            Description: socialAction.Description,
            OrganizationId: socialAction.OrganizationId,
            VolunteerCreatorId: socialAction.VolunteerCreatorId,
            State: socialAction.State,
            Interests: socialAction.Interests,
            OccurrenceDate: socialAction.OccurrenceDate,
            CreatedAt: socialAction.CreatedAt);
    
    public static GetSocialActionsItem MapToGetSocialActionsItem(this SocialAction socialAction)
        => new (
            SocialActionId: socialAction.SocialActionId,
            Title: socialAction.Title,
            Description: socialAction.Description,
            OrganizationId: socialAction.OrganizationId,
            VolunteerCreatorId: socialAction.VolunteerCreatorId,
            State: socialAction.State,
            Interests: socialAction.Interests,
            OccurrenceDate: socialAction.OccurrenceDate,
            CreatedAt: socialAction.CreatedAt);
}