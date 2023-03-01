using Colabora.Application.Commons;
using Colabora.Application.Features.GetSocialActionById.Models;
using MediatR;

namespace Colabora.Application.Features.GetSocialActionById;

public interface IGetSocialActionByIdQueryHandler : IRequestHandler<GetSocialActionByIdQuery, Result<GetSocialActionByIdResponse>>
{
    
}