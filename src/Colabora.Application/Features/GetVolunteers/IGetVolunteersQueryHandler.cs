using Colabora.Application.Commons;
using Colabora.Application.Features.GetVolunteers.Models;
using MediatR;

namespace Colabora.Application.Features.GetVolunteers;

public interface IGetVolunteersQueryHandler : IRequestHandler<GetVolunteersQuery, Result<GetVolunteersResponse>>
{
    
}