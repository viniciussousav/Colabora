using Colabora.Application.Commons;
using Colabora.Application.Features.RegisterVolunteer.Models;
using MediatR;

namespace Colabora.Application.Features.RegisterVolunteer;

public interface IRegisterVolunteerCommandHandler : IRequestHandler<RegisterVolunteerCommand, Result<RegisterVolunteerResponse>>
{
    
}