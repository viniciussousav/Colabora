using Colabora.Application.Commons;
using Colabora.Application.UseCases.RegisterVolunteer.Models;
using MediatR;

namespace Colabora.Application.UseCases.RegisterVolunteer;

public interface IRegisterVolunteerCommandHandler : IRequestHandler<RegisterVolunteerCommand, Result<RegisterVolunteerResponse>>
{
    
}