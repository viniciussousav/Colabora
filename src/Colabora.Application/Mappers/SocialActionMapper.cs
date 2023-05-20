using Colabora.Application.Features.SocialAction.CreateSocialAction.Models;
using Colabora.Application.Features.SocialAction.GetSocialActionById.Models;
using Colabora.Application.Features.SocialAction.GetSocialActions.Models;
using Colabora.Domain.Entities;

namespace Colabora.Application.Mappers;

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
            participations: new List<Participation>(),
            occurrenceDate: command.OccurrenceDate.ToUniversalTime(),
            createdAt: DateTimeOffset.UtcNow);
    
    public static CreateSocialActionResponse MapToCreateSocialActionResponse(this SocialAction socialAction)
        => new (
            SocialActionId: socialAction.SocialActionId,
            Title: socialAction.Title,
            Description: socialAction.Description,
            OrganizationId: socialAction.OrganizationId,
            VolunteerCreatorId: socialAction.VolunteerCreatorId,
            State: socialAction.State,
            Interests: socialAction.Interests,
            OccurrenceDate: socialAction.OccurrenceDate.ToUniversalTime(),
            CreatedAt: socialAction.CreatedAt.ToUniversalTime());
    
    public static GetSocialActionsItemResponse MapToGetSocialActionsItem(this SocialAction socialAction)
        => new (
            SocialActionId: socialAction.SocialActionId,
            Title: socialAction.Title,
            Description: socialAction.Description,
            OrganizationId: socialAction.OrganizationId,
            VolunteerCreatorId: socialAction.VolunteerCreatorId,
            State: socialAction.State,
            Interests: socialAction.Interests,
            OccurrenceDate: socialAction.OccurrenceDate.ToUniversalTime(),
            CreatedAt: socialAction.CreatedAt.ToUniversalTime());
    
    public static GetSocialActionByIdResponse MapToGetSocialActionByIdResponse(this SocialAction socialAction)
        => new (
            SocialActionId: socialAction.SocialActionId,
            Title: socialAction.Title,
            Description: socialAction.Description,
            OrganizationId: socialAction.OrganizationId,
            VolunteerCreatorId: socialAction.VolunteerCreatorId,
            State: socialAction.State,
            Interests: socialAction.Interests,
            OccurrenceDate: socialAction.OccurrenceDate.ToUniversalTime(),
            CreatedAt: socialAction.CreatedAt.ToUniversalTime(),
            Participations: socialAction.Participations.Select(
                p => new SocialActionParticipationDetails(
                    VolunteerId: p.VolunteerId, 
                    FullName: p.Volunteer.FullName,
                    JoinedAt: p.JoinedAt)).ToList());
}