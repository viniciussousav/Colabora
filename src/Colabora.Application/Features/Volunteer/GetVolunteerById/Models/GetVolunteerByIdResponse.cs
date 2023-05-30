using Colabora.Domain.Shared.Enums;
using Colabora.Domain.Volunteer;

namespace Colabora.Application.Features.Volunteer.GetVolunteerById.Models;

public record GetVolunteerByIdResponse(
    int VolunteerId,
    string FirstName,
    string LastName,
    string Email,
    States State,
    Gender Gender,
    IEnumerable<Interests> Interests,
    DateTimeOffset Birthdate,
    DateTimeOffset CreatedAt,
    IEnumerable<VolunteerParticipationDetails> Participations);

public record VolunteerParticipationDetails(
    int SocialActionId,
    string SocialActionTitle,
    DateTimeOffset JoinedAt,
    DateTimeOffset OccurrenceDate);
