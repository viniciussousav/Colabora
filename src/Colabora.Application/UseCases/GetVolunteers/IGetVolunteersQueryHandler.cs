using Colabora.Application.Commons;
using Colabora.Application.UseCases.GetVolunteers.Models;
using MediatR;

namespace Colabora.Application.UseCases.GetVolunteers;

public interface IGetVolunteersQueryHandler : IRequestHandler<GetVolunteersQuery, Result<GetVolunteersResponse>>
{
    
}