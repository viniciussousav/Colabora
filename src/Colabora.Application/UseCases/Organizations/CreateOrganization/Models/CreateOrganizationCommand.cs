using Colabora.Domain.Entities.Organizations.ValueObjects;
using Colabora.Domain.Entities.Volunteers;
using Colabora.Domain.Shared.Enums;

namespace Colabora.Application.UseCases.Organizations.CreateOrganization.Models;

public record CreateOrganizationCommand(
    int Id,
    string Name,
    States State,
    List<Interests> Interests,
    List<Membership> Memberships,
    Volunteer CreatedBy,
    string CreatedAt
);
