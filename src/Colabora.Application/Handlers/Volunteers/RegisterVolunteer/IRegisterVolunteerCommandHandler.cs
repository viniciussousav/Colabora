using Colabora.Application.Commons;
using Colabora.Application.Handlers.Volunteers.RegisterVolunteer.Models;
using MediatR;

namespace Colabora.Application.Handlers.Volunteers.RegisterVolunteer;

public interface IRegisterVolunteerCommandHandler : IRequestHandler<RegisterVolunteerCommand, Result<RegisterVolunteerResponse>>
{
    
}