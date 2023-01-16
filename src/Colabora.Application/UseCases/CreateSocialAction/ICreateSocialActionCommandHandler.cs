using Colabora.Application.Commons;
using Colabora.Application.UseCases.CreateSocialAction.Models;
using MediatR;

namespace Colabora.Application.UseCases.CreateSocialAction;

public interface ICreateSocialActionCommandHandler : IRequestHandler<CreateSocialActionCommand, Result<CreateSocialActionResponse>>
{
    
}