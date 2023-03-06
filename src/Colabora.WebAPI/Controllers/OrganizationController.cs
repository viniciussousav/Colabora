using Colabora.Application.Features.GetOrganizationById;
using Colabora.Application.Features.RegisterOrganization.Models;
using MediatR;
using Microsoft.AspNetCore.Mvc;

namespace Colabora.WebAPI.Controllers;

[ApiController]
[ApiVersion("1.0")]
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
            : StatusCode(result.FailureStatusCode, result.Error);
    }
    
    [HttpPost]
    public async Task<IActionResult> RegisterOrganization([FromBody] RegisterOrganizationCommand command)
    {
        var result = await _mediator.Send(command);
        
        return result.IsValid
            ? Ok(result.Value)
            : StatusCode(result.FailureStatusCode, result.Error);
    }
}