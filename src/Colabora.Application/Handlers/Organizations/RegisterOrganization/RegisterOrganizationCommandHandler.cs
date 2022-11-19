using Colabora.Application.Commons;
using Colabora.Application.Handlers.Organizations.RegisterOrganization.Mappers;
using Colabora.Application.Handlers.Organizations.RegisterOrganization.Models;
using Colabora.Domain.Entities;
using Colabora.Domain.Repositories;
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

    public async Task<Result<RegisterOrganizationResponse>> Handle(RegisterOrganizationCommand command, CancellationToken cancellationToken)
    {
        try
        {
            if (await OrganizationAlreadyExist(command.Name, command.CreatedBy))
                return Result.Fail<RegisterOrganizationResponse>(ErrorMessages.CreateOrganizationConflict(command.Name));

            var organization = await _organizationRepository.CreateOrganization(command.MapToOrganization());

            return Result.Success(organization.MapToResponse());
        }
        catch (Exception e)
        {
            _logger.LogError(e, "An exception was throw at {CreateOrganizationHandler}", nameof(RegisterOrganizationCommandHandler));
            return Result.Fail<RegisterOrganizationResponse>(ErrorMessages.CreateUnexpectedErrorMessage(e.Message));
        }
    }
    
    private async Task<bool> OrganizationAlreadyExist(string name, int volunteerCreatorId)
        => await _organizationRepository.GetOrganizationByNameAndCreator(name,volunteerCreatorId) != Organization.Empty;

}