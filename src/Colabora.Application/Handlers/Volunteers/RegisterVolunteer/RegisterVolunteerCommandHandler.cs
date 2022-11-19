using System.Net;
using Colabora.Application.Commons;
using Colabora.Application.Handlers.Volunteers.RegisterVolunteer.Mappers;
using Colabora.Application.Handlers.Volunteers.RegisterVolunteer.Models;
using Colabora.Domain.Entities;
using Colabora.Domain.Repositories;
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
    
    public async Task<Result<RegisterVolunteerResponse>> Handle(RegisterVolunteerCommand command, CancellationToken cancellationToken)
    {
        try
        {
            if (await IsEmailRegistered(command.Email))
                return Result.Fail<RegisterVolunteerResponse>(ErrorMessages.CreateEmailConflictErrorMessage(command.Email), HttpStatusCode.Conflict);

            var volunteer = await _volunteerRepository.CreateVolunteer(command.MapToVolunteer());

            return Result.Success(volunteer.MapToRegisterVolunteerResponse());
        }
        catch (Exception e)
        {
            _logger.LogError(e, "An exception was throw at {IRegisterVolunteerCommandHandler}", nameof(IRegisterVolunteerCommandHandler));
            return Result.Fail<RegisterVolunteerResponse>(ErrorMessages.CreateInternalErrorException(e.Message));
        }
    }

    private async Task<bool> IsEmailRegistered(string email) => await _volunteerRepository.GetVolunteerByEmail(email) != Volunteer.Empty;
}