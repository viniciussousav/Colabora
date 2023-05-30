using Colabora.Application.Commons;
using Colabora.Application.Features.Organization.GetOrganizationById.Models;
using Colabora.Application.Mappers;
using Colabora.Application.Shared;
using Colabora.Domain.Organization;
using Microsoft.AspNetCore.Http;
using Microsoft.Extensions.Logging;

namespace Colabora.Application.Features.Organization.GetOrganizationById;

public class GetOrganizationByIdQueryHandler : IGetOrganizationByIdQueryHandler
{
    private readonly ILogger<GetOrganizationByIdQueryHandler> _logger;
    private readonly IOrganizationRepository _organizationRepository;

    public GetOrganizationByIdQueryHandler(ILogger<GetOrganizationByIdQueryHandler> logger,
        IOrganizationRepository organizationRepository)
    {
        _logger = logger;
        _organizationRepository = organizationRepository;
    }

    public async Task<Result<GetOrganizationByIdResponse>> Handle(GetOrganizationByIdQuery query, CancellationToken cancellationToken)
    {
        try
        {
            var organization = await _organizationRepository.GetOrganizationById(query.Id, true);

            if (organization == Domain.Organization.Organization.None)
            {
                return Result.Fail<GetOrganizationByIdResponse>(
                    error: ErrorMessages.CreateOrganizationNotFound(),
                    failureStatusCode: StatusCodes.Status404NotFound);
            }

            var response = organization.MapToGetOrganizationByIdResponse();
            return Result.Success(response);
        }
        catch (Exception e)
        {
            _logger.LogError(e, "An exception was throw at {GetOrganizationByIdQueryHandler}", nameof(GetOrganizationByIdQueryHandler));
            return Result.Fail<GetOrganizationByIdResponse>(
                error: ErrorMessages.CreateInternalError(e.Message),
                failureStatusCode: StatusCodes.Status500InternalServerError);
        }
    }
}