using Colabora.Domain.Enums;

namespace Colabora.Application.UseCases.GetVolunteers.Models;

public record GetVolunteersItemResponse(
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