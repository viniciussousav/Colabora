using Colabora.Application.Features.Volunteer.GetVolunteerById.Models;
using Colabora.Application.Features.Volunteer.GetVolunteers.Models;
using Colabora.Application.Features.Volunteer.RegisterVolunteer.Models;
using MediatR;
using Microsoft.AspNetCore.Mvc;

namespace Colabora.WebAPI.Controllers.v1;

[ApiController]
[ApiVersion("1")]
[Route("api/v{version:apiVersion}/volunteers")]
public class VolunteerController : ControllerBase
{
    private readonly IMediator _mediator;
    
    public VolunteerController(IMediator mediator)
    {
        _mediator = mediator;
    }
    
    [HttpGet]
    public async Task<IActionResult> GetVolunteers()
    {
        var result = await _mediator.Send(new GetVolunteersQuery());

        return result.IsValid  
            ? Ok(result.Value)
            : StatusCode(result.FailureStatusCode, result.Errors);
    }
    
    [HttpGet("{id:int}")]
    public async Task<IActionResult> GetVolunteerById([FromRoute] int id)
    {
        var result = await _mediator.Send(new GetVolunteerByIdQuery(id));

        return result.IsValid  
            ? Ok(result.Value)
            : StatusCode(result.FailureStatusCode, result.Errors);
    }
    
    [HttpPost]
    public async Task<IActionResult> RegisterVolunteer([FromBody] RegisterVolunteerCommand command)
    {
        var result = await _mediator.Send(command);
        
        return result.IsValid  
            ? Ok(result.Value)
            : StatusCode(result.FailureStatusCode, result.Errors);
    }
}