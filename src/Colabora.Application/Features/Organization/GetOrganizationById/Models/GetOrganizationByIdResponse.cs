using Colabora.Domain.Shared.Enums;

namespace Colabora.Application.Features.Organization.GetOrganizationById.Models;

public record GetOrganizationByIdResponse(
    int OrganizationId,
    string Name,
    States State,
    IEnumerable<Interests> Interests,
    int CreatedBy,
    DateTimeOffset CreatedAt,
    bool Verified,
    List<OrganizationSocialActionDetails> SocialActions);
    
public record OrganizationSocialActionDetails(
    int SocialActionId,
    string SocialActionTitle,
    DateTimeOffset CreatedAt,
    DateTimeOffset OccurrenceDate);