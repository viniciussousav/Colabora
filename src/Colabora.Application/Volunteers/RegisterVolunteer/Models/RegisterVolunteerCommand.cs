using Colabora.Application.Commons;
using Colabora.Domain.Enums;
using MediatR;

namespace Colabora.Application.Volunteers.RegisterVolunteer.Models;

public record RegisterVolunteerCommand(
    string FirstName,
    string LastName,
    string Email,
    States State,
    Gender Gender,
    List<Interests> Interests,
    DateTime Birthdate
) : IRequest<Result<RegisterVolunteerResponse>>;