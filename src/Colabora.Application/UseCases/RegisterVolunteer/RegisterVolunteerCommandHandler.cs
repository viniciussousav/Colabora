using Colabora.Application.Commons;
using Colabora.Application.Shared;
using Colabora.Application.Shared.Mappers;
using Colabora.Application.UseCases.RegisterVolunteer.Models;
using Colabora.Domain.Entities;
using Colabora.Domain.Repositories;
using Microsoft.Extensions.Logging;

namespace Colabora.Application.UseCases.RegisterVolunteer;

public class RegisterVolunteerCommandHandler : IRegisterVolunteerCommandHandler
{
    private readonly ILogger<RegisterVolunteerCommandHandler> _logger;
    private readonly IVolunteerRepository _volunteerRepository;
    
    public RegisterVolunteerCommandHandler( ILogger<RegisterVolunteerCommandHandler> logger, IVolunteerRepository volunteerRepository)
    {
        _logger = logger;
        _volunteerRepository = volunteerRepository;
    }
    
    public async Task<Result<RegisterVolunteerResponse>> Handle(RegisterVolunteerCommand command, CancellationToken cancellationToken)
    {
        try
        {
            if (await IsEmailRegistered(command.Email))
                return Result.Fail<RegisterVolunteerResponse>(ErrorMessages.CreateVolunteerEmailAlreadyExists(command.Email));
            
            var volunteer = command.MapToVolunteer();
            var createdVolunteer = await _volunteerRepository.CreateVolunteer(volunteer);
            
            return Result.Success(createdVolunteer.MapToRegisterVolunteerResponse());
        }
        catch (Exception e)
        {
            _logger.LogError(e, "An exception was throw at {IRegisterVolunteerCommandHandler}", nameof(IRegisterVolunteerCommandHandler));
            return Result.Fail<RegisterVolunteerResponse>(ErrorMessages.CreateInternalError(e.Message));
        }
    }

    private async Task<bool> IsEmailRegistered(string email) => 
        await _volunteerRepository.GetVolunteerByEmail(email) != Volunteer.None;
}