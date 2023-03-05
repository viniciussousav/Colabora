using Colabora.Domain.Enums;

namespace Colabora.Application.Features.GetVolunteerById.Models;

public record GetVolunteerByIdResponse(
    int VolunteerId,
    string FirstName,
    string LastName,
    string Email,
    States State,
    Gender Gender,
    List<Interests> Interests,
    DateTime Birthdate,
    DateTime CreatedAt,
    List<VolunteerParticipationDetails> Participations);

public record VolunteerParticipationDetails(
    int SocialActionId,
    string SocialActionTitle,
    DateTimeOffset JoinedAt,
    DateTimeOffset OccurrenceDate);
