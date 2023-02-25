using Colabora.Application.Commons;
using Colabora.Application.Features.CreateSocialAction.Models;
using MediatR;

namespace Colabora.Application.Features.CreateSocialAction;

public interface ICreateSocialActionCommandHandler : IRequestHandler<CreateSocialActionCommand, Result<CreateSocialActionResponse>>
{
    
}