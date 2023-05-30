using Colabora.Application.Commons;
using Colabora.Domain.Shared.Enums;
using MediatR;

namespace Colabora.Application.Features.Organization.RegisterOrganization.Models;

public record RegisterOrganizationCommand(
    string Name,
    string Email,
    States State,
    IEnumerable<Interests> Interests,
    int VolunteerCreatorId
) : IRequest<Result<RegisterOrganizationResponse>>;
