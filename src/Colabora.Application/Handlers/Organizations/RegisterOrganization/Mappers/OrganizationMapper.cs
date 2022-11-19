using Colabora.Application.Handlers.Organizations.RegisterOrganization.Models;
using Colabora.Domain.Entities;

namespace Colabora.Application.Handlers.Organizations.RegisterOrganization.Mappers;

public static class OrganizationMapper
{
    public static RegisterOrganizationResponse MapToResponse(this Organization organization)
        => new(
            organization.Id,
            organization.Name,
            organization.State,
            organization.Interests,
            organization.Memberships,
            organization.CreatedBy,
            organization.CreatedAt);
}