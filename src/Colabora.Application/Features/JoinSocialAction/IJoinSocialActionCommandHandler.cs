using Colabora.Application.Commons;
using Colabora.Application.Features.JoinSocialAction.Models;
using MediatR;

namespace Colabora.Application.Features.JoinSocialAction;

public interface IJoinSocialActionCommandHandler : IRequestHandler<JoinSocialActionCommand, Result<JoinSocialActionResponse>>
{
    
}