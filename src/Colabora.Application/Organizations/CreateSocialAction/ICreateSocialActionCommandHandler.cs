using Colabora.Application.Commons;
using Colabora.Application.Organizations.CreateSocialAction.Models;
using MediatR;

namespace Colabora.Application.Organizations.CreateSocialAction;

public interface ICreateSocialActionCommandHandler : IRequestHandler<CreateSocialActionCommand, Result<CreateSocialActionResponse>>
{
    
}