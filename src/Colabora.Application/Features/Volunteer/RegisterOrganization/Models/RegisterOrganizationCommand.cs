using Colabora.Application.Commons;
using Colabora.Domain.Shared.Enums;
using MediatR;

namespace Colabora.Application.Features.Volunteer.RegisterOrganization.Models;

public record RegisterOrganizationCommand(
    string Name,
    string Email,
    States State,
    IEnumerable<Interests> Interests,
    Guid VolunteerCreatorId
) : IRequest<Result<RegisterOrganizationResponse>>;
