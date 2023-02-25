using Colabora.Application.Commons;
using MediatR;

namespace Colabora.Application.Features.JoinSocialAction.Models;

public record JoinSocialActionCommand(
    int SocialActionId,
    int VolunteerId) 
    : IRequest<Result<JoinSocialActionResponse>>;