using Colabora.Application.UseCases.GetVolunteers.Models;
using Colabora.Application.UseCases.RegisterVolunteer.Models;
using MediatR;
using Microsoft.AspNetCore.Mvc;

namespace Colabora.WebAPI.Controllers;

[ApiController]
[ApiVersion("1.0")]
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
            : StatusCode(result.FailureStatusCode, result.Error);
    }
    
    [HttpPost]
    public async Task<IActionResult> RegisterVolunteer(RegisterVolunteerCommand command)
    {
        var result = await _mediator.Send(command);
            
        return result.IsValid  
            ? Ok(result.Value)
            : StatusCode(result.FailureStatusCode, result.Error);
    }
}