using Colabora.Application.Commons;
using Colabora.Application.Features.SocialAction.CreateSocialAction.Models;
using MediatR;

namespace Colabora.Application.Features.SocialAction.CreateSocialAction;

public interface ICreateSocialActionCommandHandler : IRequestHandler<CreateSocialActionCommand, Result<CreateSocialActionResponse>>
{
    
}