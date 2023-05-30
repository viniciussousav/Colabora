using Colabora.Domain.Shared.Enums;

namespace Colabora.Application.Features.Organization.RegisterOrganization.Models;

public record RegisterOrganizationResponse(
    int OrganizationId,
    string Name,
    string Email,
    States State,
    IEnumerable<Interests> Interests,
    int CreatedBy,
    DateTimeOffset CreatedAt,
    bool Verified);