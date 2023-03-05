using Colabora.Application.Commons;
using Colabora.Application.Features.GetVolunteerById.Models;
using MediatR;

namespace Colabora.Application.Features.GetVolunteerById;

public interface IGetVolunteerByIdQueryHandler : IRequestHandler<GetVolunteerByIdQuery, Result<GetVolunteerByIdResponse>>
{
    
}