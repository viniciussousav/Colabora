using Colabora.Domain.Shared.Enums;
using Colabora.Domain.Volunteer;

namespace Colabora.Application.Features.Volunteer.GetVolunteers.Models;

public record GetVolunteersItemResponse(
    Guid VolunteerId,
    string FirstName,
    string LastName,
    string Email,
    States State,
    Gender Gender,
    IEnumerable<Interests> Interests,
    DateTimeOffset Birthdate,
    DateTimeOffset CreatedAt
);