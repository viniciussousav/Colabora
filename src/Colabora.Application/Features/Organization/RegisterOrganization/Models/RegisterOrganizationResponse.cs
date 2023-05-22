using Colabora.Domain.Enums;

namespace Colabora.Application.Features.Organization.RegisterOrganization.Models;

public record RegisterOrganizationResponse(
    int OrganizationId,
    string Name,
    States State,
    IEnumerable<Interests> Interests,
    int CreatedBy,
    DateTimeOffset CreatedAt,
    bool Verified);