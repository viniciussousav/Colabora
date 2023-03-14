using Colabora.Application.Commons;
using Colabora.Application.Features.Organization.GetOrganizationById.Models;
using MediatR;

namespace Colabora.Application.Features.Organization.GetOrganizationById;

public interface IGetOrganizationByIdQueryHandler : IRequestHandler<GetOrganizationByIdQuery, Result<GetOrganizationByIdResponse>>
{
    
}