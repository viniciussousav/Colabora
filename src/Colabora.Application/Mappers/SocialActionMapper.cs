using Colabora.Application.Features.CreateSocialAction.Models;
using Colabora.Application.Features.GetSocialActionById.Models;
using Colabora.Application.Features.GetSocialActions.Models;
using Colabora.Domain.Entities;
using Colabora.Domain.ValueObjects;

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
    
    public static GetSocialActionsItemResponse MapToGetSocialActionsItem(this SocialAction socialAction)
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
    
    public static GetSocialActionByIdResponse MapToGetSocialActionByIdResponse(this SocialAction socialAction)
        => new (
            SocialActionId: socialAction.SocialActionId,
            Title: socialAction.Title,
            Description: socialAction.Description,
            OrganizationId: socialAction.OrganizationId,
            VolunteerCreatorId: socialAction.VolunteerCreatorId,
            State: socialAction.State,
            Interests: socialAction.Interests,
            OccurrenceDate: socialAction.OccurrenceDate,
            CreatedAt: socialAction.CreatedAt,
            Participations: socialAction.Participations.Select(
                p => new ParticipationDetails(
                    VolunteerId: p.VolunteerId, 
                    FullName: $"{p.Volunteer.FullName}",
                    JoinedAt: p.JoinedAt)).ToList());
}