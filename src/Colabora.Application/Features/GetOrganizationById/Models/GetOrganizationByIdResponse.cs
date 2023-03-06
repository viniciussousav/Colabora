using Colabora.Domain.Enums;

namespace Colabora.Application.Features.GetOrganizationById.Models;

public record GetOrganizationByIdResponse(
    int OrganizationId,
    string Name,
    States State,
    List<Interests> Interests,
    int CreatedBy,
    DateTime CreatedAt,
    List<OrganizationSocialActionDetails> SocialActions);
    
public record OrganizationSocialActionDetails(
    int SocialActionId,
    string SocialActionTitle,
    DateTimeOffset CreatedAt,
    DateTimeOffset OccurrenceDate);