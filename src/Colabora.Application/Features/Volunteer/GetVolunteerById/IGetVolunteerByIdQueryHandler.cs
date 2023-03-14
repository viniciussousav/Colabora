using Colabora.Application.Commons;
using Colabora.Application.Features.Volunteer.GetVolunteerById.Models;
using MediatR;

namespace Colabora.Application.Features.Volunteer.GetVolunteerById;

public interface IGetVolunteerByIdQueryHandler : IRequestHandler<GetVolunteerByIdQuery, Result<GetVolunteerByIdResponse>>
{
    
}