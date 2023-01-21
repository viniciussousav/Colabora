using Colabora.Application.UseCases.CreateSocialAction.Models;
using MediatR;
using Microsoft.AspNetCore.Mvc;

namespace Colabora.WebAPI.Controllers;

[ApiController]
[Route("api/v{version:apiVersion}/actions")]
[ApiVersion("1.0")]
public class SocialActionController : ControllerBase
{
    private readonly IMediator _mediator;
    
    public SocialActionController(IMediator mediator)
    {
        _mediator = mediator;
    }

    [HttpPost]
    public async Task<IActionResult> CreateSocialAction(CreateSocialActionCommand command)
    {
        var result = await _mediator.Send(command);

        return result.IsValid
            ? Ok(result.Value)
            : StatusCode(result.FailureStatusCode, result.Error);
    } 
}