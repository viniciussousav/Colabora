using Colabora.Domain.Enums;

namespace Colabora.Application.Features.Volunteer.RegisterVolunteer.Models;

public record RegisterVolunteerResponse(
    int VolunteerId,
    string FirstName,
    string LastName,
    string Email,
    States State,
    Gender Gender,
    IEnumerable<Interests> Interests,
    DateTime Birthdate,
    DateTime CreatedAt
);