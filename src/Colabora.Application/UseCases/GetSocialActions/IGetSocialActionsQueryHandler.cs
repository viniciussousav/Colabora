using Colabora.Application.Commons;
using Colabora.Application.UseCases.GetSocialActions.Models;
using MediatR;

namespace Colabora.Application.UseCases.GetSocialActions;

public interface IGetSocialActionsQueryHandler : IRequestHandler<GetSocialActionsQuery, Result<GetSocialActionsResponse>>
{
    
}