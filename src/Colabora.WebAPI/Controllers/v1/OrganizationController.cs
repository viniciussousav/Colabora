using Colabora.Application.Features.Organization.GetOrganizationById.Models;
using Colabora.Application.Features.Organization.RegisterOrganization.Models;
using MediatR;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace Colabora.WebAPI.Controllers.v1;

[Authorize]
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

    [HttpGet("{id:int}")]
    public async Task<IActionResult> GetOrganizationById([FromRoute] int id)
    {
        var result = await _mediator.Send(new GetOrganizationByIdQuery(id));

        return result.IsValid
            ? Ok(result.Value)
            : StatusCode(result.FailureStatusCode, result.Errors);
    }

    [HttpPost, Authorize]
    public async Task<IActionResult> RegisterOrganization([FromBody] RegisterOrganizationCommand command)
    {
        var result = await _mediator.Send(command);

        if (!result.IsValid)
            return StatusCode(result.FailureStatusCode, result.Errors);

        return CreatedAtAction(nameof(GetOrganizationById), new {Id = result.Value!.OrganizationId}, result.Value);
    }
}