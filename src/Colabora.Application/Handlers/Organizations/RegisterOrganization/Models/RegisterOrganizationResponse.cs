using Colabora.Domain.Entities;
using Colabora.Domain.Enums;

namespace Colabora.Application.Handlers.Organizations.RegisterOrganization.Models;

public record RegisterOrganizationResponse(
    int Id,
    string Name,
    States State,
    List<Interests> Interests,
    List<Membership> Memberships,
    int CreatedBy,
    DateTime CreatedAt);