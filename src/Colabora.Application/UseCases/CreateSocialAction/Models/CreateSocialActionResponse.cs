using Colabora.Domain.Enums;

namespace Colabora.Application.UseCases.CreateSocialAction.Models;

public record CreateSocialActionResponse(
    int SocialActionId,
    string Title,
    string Description,
    int OrganizationId,
    int VolunteerCreatorId,
    States State,
    Interests Interests,
    DateTimeOffset OccurrenceDate,
    DateTimeOffset CreatedAt);