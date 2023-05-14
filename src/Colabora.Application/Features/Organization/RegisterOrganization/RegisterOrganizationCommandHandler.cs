using Colabora.Application.Commons;
using Colabora.Application.Features.Organization.RegisterOrganization.Models;
using Colabora.Application.Mappers;
using Colabora.Application.Services.EmailVerification;
using Colabora.Application.Shared;
using Colabora.Domain.Repositories;
using FluentValidation;
using Microsoft.AspNetCore.Http;
using Microsoft.Extensions.Logging;

namespace Colabora.Application.Features.Organization.RegisterOrganization;

public class RegisterOrganizationCommandHandler : IRegisterOrganizationCommandHandler
{
    private readonly ILogger<RegisterOrganizationCommandHandler> _logger;
    private readonly IOrganizationRepository _organizationRepository;
    private readonly IVolunteerRepository _volunteerRepository;
    private readonly IEmailVerificationService _emailVerificationService;
    private readonly IValidator<RegisterOrganizationCommand> _validator;

    public RegisterOrganizationCommandHandler(
        ILogger<RegisterOrganizationCommandHandler> logger,
        IOrganizationRepository organizationRepository, 
        IVolunteerRepository volunteerRepository, 
        IEmailVerificationService emailVerificationService,
        IValidator<RegisterOrganizationCommand> validator)
    {
        _logger = logger;
        _organizationRepository = organizationRepository;
        _volunteerRepository = volunteerRepository;
        _emailVerificationService = emailVerificationService;
        _validator = validator;
    }

    public async Task<Result<RegisterOrganizationResponse>> Handle(RegisterOrganizationCommand command, CancellationToken cancellationToken)
    {
        try
        {
            var validationResult = await _validator.ValidateAsync(command, cancellationToken);

            if (!validationResult.IsValid)
                return Result.Fail<RegisterOrganizationResponse>(validationResult.Errors);

            if (await OrganizationEmailAlreadyExist(command.Name, command.Email, command.VolunteerCreatorId))
            {
                return Result.Fail<RegisterOrganizationResponse>(
                    error: ErrorMessages.CreateOrganizationEmailAlreadyExists(command.Name), 
                    failureStatusCode: StatusCodes.Status409Conflict);
            }

            if (await VolunteerCreatorNotExists(command.VolunteerCreatorId))
            {
                return Result.Fail<RegisterOrganizationResponse>(
                    error: ErrorMessages.CreateVolunteerNotFound(),
                    failureStatusCode: StatusCodes.Status404NotFound);
            }

            await _emailVerificationService.SendEmailVerificationRequest(command.Email, cancellationToken);
            
            var organization = command.MapToOrganization();
            var createdOrganization = await _organizationRepository.CreateOrganization(organization);

            var response = createdOrganization.MapToRegisterOrganizationResponse();

            return Result.Success(response);
        }
        catch (Exception e)
        {
            _logger.LogError(e, "An exception was throw at {CreateOrganizationHandler}", nameof(RegisterOrganizationCommandHandler));
            return Result.Fail<RegisterOrganizationResponse>(
                error: ErrorMessages.CreateInternalError(e.Message),
                failureStatusCode: StatusCodes.Status500InternalServerError);
        }
    }

    private async Task<bool> VolunteerCreatorNotExists(int volunteerId)
        => await _volunteerRepository.GetVolunteerById(volunteerId) == Domain.Entities.Volunteer.None;
    
    private async Task<bool> OrganizationEmailAlreadyExist(string name, string email, int volunteerCreator)
        => await _organizationRepository.GetOrganization(name, email, volunteerCreator) != Domain.Entities.Organization.None;

}