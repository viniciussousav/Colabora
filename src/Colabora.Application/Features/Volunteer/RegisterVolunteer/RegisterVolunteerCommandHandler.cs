using Colabora.Application.Commons;
using Colabora.Application.Features.Volunteer.RegisterVolunteer.Models;
using Colabora.Application.Mappers;
using Colabora.Application.Shared;
using Colabora.Domain.Repositories;
using FluentValidation;
using Microsoft.AspNetCore.Http;
using Microsoft.Extensions.Logging;

namespace Colabora.Application.Features.Volunteer.RegisterVolunteer;

public class RegisterVolunteerCommandHandler : IRegisterVolunteerCommandHandler
{
    private readonly ILogger<RegisterVolunteerCommandHandler> _logger;
    private readonly IVolunteerRepository _volunteerRepository;
    private readonly IValidator<RegisterVolunteerCommand> _validator;

    public RegisterVolunteerCommandHandler( 
        ILogger<RegisterVolunteerCommandHandler> logger, 
        IVolunteerRepository volunteerRepository, 
        IValidator<RegisterVolunteerCommand> validator)
    {
        _logger = logger;
        _volunteerRepository = volunteerRepository;
        _validator = validator;
    }
    
    public async Task<Result<RegisterVolunteerResponse>> Handle(RegisterVolunteerCommand command, CancellationToken cancellationToken)
    {
        try
        {
            var validationResult = await _validator.ValidateAsync(command, cancellationToken);

            if (!validationResult.IsValid)
                return Result.Fail<RegisterVolunteerResponse>(validationResult.Errors);
            
            if (await IsEmailRegistered(command.Email))
                return Result.Fail<RegisterVolunteerResponse>(
                    error: ErrorMessages.CreateVolunteerEmailAlreadyExists(command.Email),
                    failureStatusCode: StatusCodes.Status409Conflict);
            
            var volunteer = command.MapToVolunteer();
            var createdVolunteer = await _volunteerRepository.CreateVolunteer(volunteer);
            
            return Result.Success(createdVolunteer.MapToRegisterVolunteerResponse());
        }
        catch (Exception e)
        {
            _logger.LogError(e, "An exception was throw at {IRegisterVolunteerCommandHandler}", nameof(IRegisterVolunteerCommandHandler));
            return Result.Fail<RegisterVolunteerResponse>(
                error: ErrorMessages.CreateInternalError(e.Message),
                failureStatusCode: StatusCodes.Status500InternalServerError);
        }
    }

    private async Task<bool> IsEmailRegistered(string email) => 
        await _volunteerRepository.GetVolunteerByEmail(email) != Domain.Entities.Volunteer.None;
}