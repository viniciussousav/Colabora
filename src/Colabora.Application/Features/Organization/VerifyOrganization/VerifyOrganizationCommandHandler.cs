using Colabora.Application.Commons;
using Colabora.Application.Features.Organization.VerifyOrganization.Models;
using Colabora.Application.Shared;
using Colabora.Domain.Organization;
using Microsoft.Extensions.Logging;

namespace Colabora.Application.Features.Organization.VerifyOrganization;

public class VerifyOrganizationCommandHandler : IVerifyOrganizationCommandHandler
{
    private readonly ILogger<VerifyOrganizationCommandHandler> _logger;
    private readonly IOrganizationRepository _organizationRepository;

    public VerifyOrganizationCommandHandler(IOrganizationRepository organizationRepository, ILogger<VerifyOrganizationCommandHandler> logger)
    {
        _organizationRepository = organizationRepository;
        _logger = logger;
    }

    public async Task<Result<VerifyOrganizationResponse>> Handle(VerifyOrganizationCommand command, CancellationToken cancellationToken)
    {
        try
        {
            var organization = await _organizationRepository.GetOrganizationById(command.OrganizationId);

            if (organization == Domain.Organization.Organization.None)
                return Result.Fail<VerifyOrganizationResponse>(ErrorMessages.CreateOrganizationNotFound());
            
            organization.Verify();
            await _organizationRepository.UpdateOrganization(organization);
            
            return Result.Success(new VerifyOrganizationResponse());
        }
        catch (Exception e)
        {
            _logger.LogError(e, "An exception was throw at {VerifyOrganizationCommandHandler}", nameof(VerifyOrganizationCommandHandler));
            return Result.Fail<VerifyOrganizationResponse>(ErrorMessages.CreateInternalError(e.Message));
        }
    }
}