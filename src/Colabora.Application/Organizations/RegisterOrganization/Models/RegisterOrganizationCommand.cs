using Colabora.Application.Commons;
using Colabora.Domain.Entities;
using Colabora.Domain.Enums;
using MediatR;

namespace Colabora.Application.Organizations.RegisterOrganization.Models;

public record RegisterOrganizationCommand(
    string Name,
    string Email,
    States State,
    List<Interests> Interests,
    List<Membership> Memberships,
    int CreatedBy
) : IRequest<Result<RegisterOrganizationResponse>>;
