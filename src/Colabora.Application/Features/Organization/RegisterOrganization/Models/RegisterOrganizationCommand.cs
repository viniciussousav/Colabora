using Colabora.Application.Commons;
using Colabora.Domain.Enums;
using MediatR;

namespace Colabora.Application.Features.Organization.RegisterOrganization.Models;

public record RegisterOrganizationCommand(
    string Name,
    string Email,
    States State,
    List<Interests> Interests,
    int VolunteerCreatorId
) : IRequest<Result<RegisterOrganizationResponse>>;
