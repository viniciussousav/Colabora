using Colabora.Domain.Enums;

namespace Colabora.Application.Volunteers;

public record VolunteerResponse(
    int Id,
    string FirstName,
    string LastName,
    string Email,
    States State,
    Gender Gender,
    List<Interests> Interests,
    DateTime Birthdate,
    DateTime CreatedAt
);