using Colabora.Application.Commons;
using Colabora.Application.Features.Volunteer.GetVolunteers.Models;
using MediatR;

namespace Colabora.Application.Features.Volunteer.GetVolunteers;

public interface IGetVolunteersQueryHandler : IRequestHandler<GetVolunteersQuery, Result<GetVolunteersResponse>>
{
    
}