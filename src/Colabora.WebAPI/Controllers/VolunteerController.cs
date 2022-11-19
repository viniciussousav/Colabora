using Colabora.Application.Handlers.Volunteers.GetVolunteers.Models;
using MediatR;
using Microsoft.AspNetCore.Mvc;

namespace Colabora.WebAPI.Controllers;

[ApiController]
[Route("volunteers")]
public class VolunteerController : ControllerBase
{
    private readonly ILogger<VolunteerController> _logger;
    private readonly IMediator _mediator;
    
    public VolunteerController(ILogger<VolunteerController> logger, IMediator mediator)
    {
        _logger = logger;
        _mediator = mediator;
    }

    [HttpGet]
    public async Task<IActionResult> Get()
    {
        try
        {
            var result = await _mediator.Send(new GetVolunteersQuery());

            return result.IsValid  
                ? Ok(result.Value)
                : StatusCode(result.FailureStatusCode, result.Errors);
        }
        catch (Exception e)
        {
            _logger.LogError(e, "An exception was throw at {VolunteerController}", nameof(VolunteerController));
            return Problem(detail: e.Message, statusCode: 500, title: "Internal Error");
        }
    }
}