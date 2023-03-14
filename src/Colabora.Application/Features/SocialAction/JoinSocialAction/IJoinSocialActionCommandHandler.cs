using Colabora.Application.Commons;
using Colabora.Application.Features.SocialAction.JoinSocialAction.Models;
using MediatR;

namespace Colabora.Application.Features.SocialAction.JoinSocialAction;

public interface IJoinSocialActionCommandHandler : IRequestHandler<JoinSocialActionCommand, Result<JoinSocialActionResponse>>
{
    
}