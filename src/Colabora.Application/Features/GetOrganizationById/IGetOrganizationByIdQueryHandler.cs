using Colabora.Application.Commons;
using MediatR;

namespace Colabora.Application.Features.GetOrganizationById;

public interface IGetOrganizationByIdQueryHandler : IRequestHandler<GetOrganizationByIdQuery, Result<GetOrganizationByIdResponse>>
{
    
}