using Colabora.Application.Commons;
using Colabora.Application.Handlers.Volunteers.GetVolunteers.Models;
using MediatR;

namespace Colabora.Application.Handlers.Volunteers.GetVolunteers;

public interface IGetVolunteersQueryHandler : IRequestHandler<GetVolunteersQuery, Result<GetVolunteersResponse>>
{
    
}