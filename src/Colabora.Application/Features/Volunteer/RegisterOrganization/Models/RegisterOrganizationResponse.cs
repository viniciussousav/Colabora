using Colabora.Domain.Shared.Enums;

namespace Colabora.Application.Features.Volunteer.RegisterOrganization.Models;

public record RegisterOrganizationResponse(
    Guid OrganizationId,
    string Name,
    string Email,
    States State,
    IEnumerable<Interests> Interests,
    Guid VolunteerCreatorId,
    DateTimeOffset CreatedAt);