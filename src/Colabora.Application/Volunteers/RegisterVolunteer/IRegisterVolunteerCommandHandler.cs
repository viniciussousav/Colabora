using Colabora.Application.Commons;
using Colabora.Application.Volunteers.RegisterVolunteer.Models;
using MediatR;

namespace Colabora.Application.Volunteers.RegisterVolunteer;

public interface IRegisterVolunteerCommandHandler : IRequestHandler<RegisterVolunteerCommand, Result<RegisterVolunteerResponse>>
{
    
}