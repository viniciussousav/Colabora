using Colabora.Application.Commons;
using Colabora.Application.Features.SocialAction.GetSocialActionById.Models;
using MediatR;

namespace Colabora.Application.Features.SocialAction.GetSocialActionById;

public interface IGetSocialActionByIdQueryHandler : IRequestHandler<GetSocialActionByIdQuery, Result<GetSocialActionByIdResponse>>
{
    
}