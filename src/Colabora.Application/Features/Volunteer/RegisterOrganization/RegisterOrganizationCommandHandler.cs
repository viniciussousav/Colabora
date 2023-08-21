using Colabora.Application.Commons;
using Colabora.Application.Features.Volunteer.RegisterOrganization.Models;
using Colabora.Application.Mappers;
using Colabora.Application.Shared;
using Colabora.Domain.Organization;
using Colabora.Domain.Volunteer;
using Colabora.Infrastructure.Messaging;
using Colabora.Infrastructure.Messaging.Producer;
using FluentValidation;
using Microsoft.AspNetCore.Http;
using Microsoft.Extensions.Logging;

namespace Colabora.Application.Features.Volunteer.RegisterOrganization;

public class RegisterOrganizationCommandHandler : IRegisterOrganizationCommandHandler
{
    private readonly ILogger<RegisterOrganizationCommandHandler> _logger;
    private readonly IOrganizationRepository _organizationRepository;
    private readonly IVolunteerRepository _volunteerRepository;
    private readonly IValidator<RegisterOrganizationCommand> _validator;
    private readonly IMessageProducer _messageProducer;

    public RegisterOrganizationCommandHandler(
        ILogger<RegisterOrganizationCommandHandler> logger,
        IOrganizationRepository organizationRepository, 
        IVolunteerRepository volunteerRepository, 
        IMessageProducer messageProducer,
        IValidator<RegisterOrganizationCommand> validator)
    {
        _logger = logger;
        _organizationRepository = organizationRepository;
        _volunteerRepository = volunteerRepository;
        _messageProducer = messageProducer;
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
            
            var organization = await _organizationRepository.CreateOrganization(command.MapToOrganization());
            var response = organization.MapToRegisterOrganizationResponse();
            
            await _messageProducer.Produce(Queues.OrganizationRegistered, response, cancellationToken);

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

    private async Task<bool> VolunteerCreatorNotExists(Guid volunteerId)
        => await _volunteerRepository.GetVolunteerById(volunteerId) == Domain.Volunteer.Volunteer.None;
    
    private async Task<bool> OrganizationEmailAlreadyExist(string name, string email, Guid volunteerCreator)
        => await _organizationRepository.GetOrganization(name, email, volunteerCreator) != Domain.Organization.Organization.None;

}