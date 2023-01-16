using Colabora.Domain.Enums;

namespace Colabora.Application.UseCases.RegisterVolunteer.Models;

public record RegisterVolunteerResponse(
    int VolunteerId,
    string FirstName,
    string LastName,
    string Email,
    States State,
    Gender Gender,
    List<Interests> Interests,
    DateTime Birthdate,
    DateTime CreatedAt
);