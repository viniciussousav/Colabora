using Colabora.Application.Commons;
using Colabora.Application.Features.Volunteer.RegisterVolunteer.Models;
using MediatR;

namespace Colabora.Application.Features.Volunteer.RegisterVolunteer;

public interface IRegisterVolunteerCommandHandler : IRequestHandler<RegisterVolunteerCommand, Result<RegisterVolunteerResponse>>
{
    
}