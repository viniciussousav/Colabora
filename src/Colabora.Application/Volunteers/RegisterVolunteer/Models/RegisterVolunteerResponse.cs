using Colabora.Domain.Enums;

namespace Colabora.Application.Volunteers.RegisterVolunteer.Models;

public record RegisterVolunteerResponse(
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