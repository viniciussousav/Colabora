using Colabora.Domain.Enums;

namespace Colabora.Application.UseCases.GetSocialActions.Models;

public record GetSocialActionsItem(
    int SocialActionId,
    string Title,
    string Description,
    int OrganizationId,
    int VolunteerCreatorId,
    States State,
    List<Interests> Interests,
    DateTimeOffset OccurrenceDate,
    DateTimeOffset CreatedAt);