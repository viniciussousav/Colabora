using Colabora.Domain.Enums;

namespace Colabora.Application.Features.SocialAction.GetSocialActions.Models;

public record GetSocialActionsItemResponse(
    int SocialActionId,
    string Title,
    string Description,
    int OrganizationId,
    int VolunteerCreatorId,
    States State,
    List<Interests> Interests,
    DateTimeOffset OccurrenceDate,
    DateTimeOffset CreatedAt);