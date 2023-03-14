using Colabora.Domain.Enums;

namespace Colabora.Application.Features.SocialAction.GetSocialActionById.Models;

public record GetSocialActionByIdResponse(
    int SocialActionId,
    string Title,
    string Description,
    int OrganizationId,
    int VolunteerCreatorId,
    States State,
    List<Interests> Interests,
    DateTimeOffset OccurrenceDate,
    DateTimeOffset CreatedAt,
    List<SocialActionParticipationDetails> Participations);

public record SocialActionParticipationDetails(
    int VolunteerId,
    string FullName,
    DateTimeOffset JoinedAt);