using Colabora.Application.Commons;
using Colabora.Application.Features.SocialAction.GetSocialActions.Models;
using MediatR;

namespace Colabora.Application.Features.SocialAction.GetSocialActions;

public interface IGetSocialActionsQueryHandler : IRequestHandler<GetSocialActionsQuery, Result<GetSocialActionsResponse>>
{
    
}