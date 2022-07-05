using MediatR;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using UpBlazor.Application.Features.TwoUp;
using UpBlazor.Core.Models;
using UpBlazor.WebApi.ViewModels;

namespace UpBlazor.WebApi.Controllers.Features;

[ApiController]
[Route("api/[controller]")]
[Authorize]
public class TwoUpController : Controller
{
    private readonly IMediator _mediator;

    public TwoUpController(IMediator mediator) => _mediator = mediator;

    [HttpPost("requests/accept")]
    [ProducesResponseType(StatusCodes.Status204NoContent)]
    public async Task<IActionResult> AcceptRequest([FromBody] TwoUpViewModel request)
    {
        await _mediator.Send(new AcceptTwoUpRequestCommand(new TwoUpRequest
        {
            RequesterId = request.RequesterId,
            RequesteeId = request.RequesteeId
        }));
        
        return NoContent();
    }
    
    [HttpPost("requests/{email}")]
    [ProducesResponseType(StatusCodes.Status204NoContent)]
    public async Task<IActionResult> RequestTwoUp(string email, [FromBody] string message)
    {
        await _mediator.Send(new CreateTwoUpRequestCommand(email, message));
        
        return NoContent();
    }
    
    [HttpGet("requests")]
    [ProducesResponseType(typeof(IEnumerable<TwoUpRequest>), StatusCodes.Status200OK)]
    public async Task<IActionResult> GetRequests()
    {
        var requests = await _mediator.Send(new GetAllTwoUpRequestsForCurrentUserQuery());
        
        return Ok(requests);
    }
    
    [HttpDelete("requests/{requesterId}")]
    [ProducesResponseType(StatusCodes.Status204NoContent)]
    public async Task<IActionResult> IgnoreRequest(string requesterId)
    {
        await _mediator.Send(new IgnoreTwoUpRequestCommand(requesterId));
        
        return NoContent();
    }
    
    [HttpGet("connections")]
    [ProducesResponseType(typeof(IEnumerable<TwoUp>), StatusCodes.Status200OK)]
    public async Task<IActionResult> GetConnections()
    {
        var connections = await _mediator.Send(new GetAllTwoUpConnectionsForCurrentUserQuery());
        
        return Ok(connections);
    }
    
    [HttpDelete("connections/{userId}")]
    [ProducesResponseType(StatusCodes.Status204NoContent)]
    public async Task<IActionResult> RemoveConnection(string userId)
    {
        await _mediator.Send(new RemoveTwoUpConnectionCommand(userId));
        
        return NoContent();
    }
    
    
}