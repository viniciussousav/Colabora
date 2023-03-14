using Colabora.Application.Commons;
using Colabora.Domain.Enums;
using MediatR;

namespace Colabora.Application.Features.SocialAction.CreateSocialAction.Models;

public record CreateSocialActionCommand(
    string Title,
    string Description,
    int OrganizationId,
    int VolunteerCreatorId,
    States State,
    List<Interests> Interests,
    DateTimeOffset OccurrenceDate
    ) : IRequest<Result<CreateSocialActionResponse>>;