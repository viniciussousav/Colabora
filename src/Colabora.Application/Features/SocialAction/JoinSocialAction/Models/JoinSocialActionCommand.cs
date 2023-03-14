using Colabora.Application.Commons;
using MediatR;

namespace Colabora.Application.Features.SocialAction.JoinSocialAction.Models;

public record JoinSocialActionCommand(
    int SocialActionId,
    int VolunteerId) 
    : IRequest<Result<JoinSocialActionResponse>>;