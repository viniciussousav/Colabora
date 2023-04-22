using Colabora.Application.Commons;
using Colabora.Domain.Enums;
using MediatR;

namespace Colabora.Application.Features.Volunteer.RegisterVolunteer.Models;

public record RegisterVolunteerCommand(
    string FirstName,
    string LastName,
    string Email,
    States State,
    Gender Gender,
    IEnumerable<Interests> Interests,
    DateTime Birthdate
) : IRequest<Result<RegisterVolunteerResponse>>;