using Colabora.Application.Commons;
using Colabora.Application.Handlers.Volunteers.RegisterVolunteer.Models;
using Colabora.Domain.Entities;
using Colabora.Domain.Repositories;
using Mapster;
using Microsoft.Extensions.Logging;

namespace Colabora.Application.Handlers.Volunteers.RegisterVolunteer;

public class RegisterVolunteerCommandHandler : IRegisterVolunteerCommandHandler
{
    private readonly ILogger<RegisterVolunteerCommandHandler> _logger;
    private readonly IVolunteerRepository _volunteerRepository;
    
    public RegisterVolunteerCommandHandler( ILogger<RegisterVolunteerCommandHandler> logger, IVolunteerRepository volunteerRepository)
    {
        _logger = logger;
        _volunteerRepository = volunteerRepository;
    }
    
    public async Task<Result<RegisterVolunteerResponse?>> Handle(RegisterVolunteerCommand command, CancellationToken cancellationToken)
    {
        try
        {
            if (await IsEmailRegistered(command.Email))
                return Result.Fail<RegisterVolunteerResponse>(ErrorMessages.CreateEmailAlreadyExists(command.Email));

            var volunteer = command.Adapt<Volunteer>();
            var createdVolunteer = await _volunteerRepository.CreateVolunteerAsync(volunteer);

            var response = createdVolunteer.Adapt<RegisterVolunteerResponse>();
            return Result.Success(response);
        }
        catch (Exception e)
        {
            _logger.LogError(e, "An exception was throw at {IRegisterVolunteerCommandHandler}", nameof(IRegisterVolunteerCommandHandler));
            return Result.Fail<RegisterVolunteerResponse>(ErrorMessages.CreateUnexpectedError(e.Message));
        }
    }

    private async Task<bool> IsEmailRegistered(string email) => await _volunteerRepository.GetVolunteerByEmailAsync(email) != Volunteer.Empty;
}