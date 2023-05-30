using Colabora.Domain.Shared.Enums;

namespace Colabora.Application.Features.SocialAction.CreateSocialAction.Models;

public record CreateSocialActionResponse(
    int SocialActionId,
    string Title,
    string Description,
    int OrganizationId,
    int VolunteerCreatorId,
    States State,
    List<Interests> Interests,
    DateTimeOffset OccurrenceDate,
    DateTimeOffset CreatedAt);