using Colabora.Application.Commons;
using Colabora.Application.Features.GetSocialActions.Models;
using MediatR;

namespace Colabora.Application.Features.GetSocialActions;

public interface IGetSocialActionsQueryHandler : IRequestHandler<GetSocialActionsQuery, Result<GetSocialActionsResponse>>
{
    
}