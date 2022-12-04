using Colabora.Application.Commons;
using Colabora.Application.Volunteers.GetVolunteers.Models;
using MediatR;

namespace Colabora.Application.Volunteers.GetVolunteers;

public interface IGetVolunteersQueryHandler : IRequestHandler<GetVolunteersQuery, Result<GetVolunteersResponse>>
{
    
}