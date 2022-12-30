using Colabora.Application.UseCases.CreateSocialAction.Models;
using Colabora.Domain.Entities;

namespace Colabora.Application.Shared.Mappers;

public static class SocialActionMapper
{
    public static SocialAction MapToSocialAction(this CreateSocialActionCommand command)
    {
        return new SocialAction(
            id: default,
            title: command.Title,
            fields: command.Fields,
            occurrenceDate: command.OccurrenceDate,
            state: command.State,
            description: command.Description,
            organizationId: command.OrganizationId,
            volunteerCreatorId: command.VolunteerCreatorId,
            organizers: command.Organizers,
            participants: command.Participants,
            createdAt: DateTime.Now);
    }

    public static CreateSocialActionResponse MapToResponse(this SocialAction socialAction)
    {
        return new CreateSocialActionResponse(
            Id: socialAction.Id,
            Title: socialAction.Title,
            Fields: socialAction.Fields,
            OccurrenceDate: socialAction.OccurrenceDate,
            State: socialAction.State,
            Description: socialAction.Description,
            OrganizationId: socialAction.OrganizationId,
            VolunteerCreatorId: socialAction.VolunteerCreatorId,
            Organizers: socialAction.Organizers,
            Participants: socialAction.Participants,
            CreatedAt: socialAction.CreatedAt);
    }
}