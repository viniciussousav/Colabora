using Colabora.Domain.Enums;

namespace Colabora.Application.Features.GetSocialActionById.Models;

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
    List<ParticipationDetails> Participations);

public record ParticipationDetails(
    int VolunteerId,
    string FullName,
    DateTimeOffset JoinedAt);