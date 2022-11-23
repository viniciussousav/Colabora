using Colabora.Application.Commons;
using Colabora.Application.Handlers.Organizations.RegisterOrganization.Models;
using Colabora.Domain.Entities;
using Colabora.Domain.Repositories;
using Mapster;
using Microsoft.Extensions.Logging;

namespace Colabora.Application.Handlers.Organizations.RegisterOrganization;

public class RegisterOrganizationCommandHandler : IRegisterOrganizationCommandHandler
{
    private readonly ILogger<RegisterOrganizationCommandHandler> _logger;
    private readonly IOrganizationRepository _organizationRepository;

    public RegisterOrganizationCommandHandler(
        ILogger<RegisterOrganizationCommandHandler> logger,
        IOrganizationRepository organizationRepository)
    {
        _logger = logger;
        _organizationRepository = organizationRepository;
    }

    public async Task<Result<RegisterOrganizationResponse?>> Handle(RegisterOrganizationCommand command, CancellationToken cancellationToken)
    {
        try
        {
            if (await OrganizationAlreadyExist(command.Name, command.CreatedBy))
                return Result.Fail<RegisterOrganizationResponse>(ErrorMessages.CreateEmailAlreadyExists(command.Name));

            var organization = command.Adapt<Organization>();
            var createdOrganization = await _organizationRepository.CreateOrganizationAsync(organization);
            
            var response = createdOrganization.Adapt<RegisterOrganizationResponse>();
            return Result.Success(response);
        }
        catch (Exception e)
        {
            _logger.LogError(e, "An exception was throw at {CreateOrganizationHandler}", nameof(RegisterOrganizationCommandHandler));
            return Result.Fail<RegisterOrganizationResponse>(ErrorMessages.CreateUnexpectedError(e.Message));
        }
    }
    
    private async Task<bool> OrganizationAlreadyExist(string name, int volunteerCreatorId)
        => await _organizationRepository.GetOrganizationByNameAndCreator(name,volunteerCreatorId) != Organization.Empty;

}