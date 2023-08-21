using Colabora.Application.Features.Organization.GetOrganizationById.Models;
using Colabora.Infrastructure.Auth.Shared;
using MediatR;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace Colabora.WebAPI.Controllers.v1;

[ApiController]
[ApiVersion("1")]
[Route("api/v{version:apiVersion}/organizations")]
public class OrganizationController : ControllerBase
{
    private readonly IMediator _mediator;

    public OrganizationController(IMediator mediator)
    {
        _mediator = mediator;
    }

    [Authorize(Policy = Policies.User)]
    [HttpGet("{organizationId:guid}", Name = "GetOrganizationById")]
    public async Task<IActionResult> GetOrganizationById([FromRoute] Guid organizationId)
    {
        var result = await _mediator.Send(new GetOrganizationByIdQuery(organizationId));

        return result.IsValid
            ? Ok(result.Value)
            : StatusCode(result.FailureStatusCode, result.Errors);
    }

    
}