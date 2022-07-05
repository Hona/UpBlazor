using MediatR;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using UpBlazor.Application.Features.Notifications;
using UpBlazor.Core.Models;

namespace UpBlazor.WebApi.Controllers.Features;

[ApiController]
[Route("api/[controller]")]
[Authorize]
public class NotificationsController : Controller
{
    private readonly IMediator _mediator;

    public NotificationsController(IMediator mediator) => _mediator = mediator;

    [HttpPost]
    [ProducesResponseType(typeof(Guid), StatusCodes.Status200OK)]
    [Authorize(Policy = Constants.AdminAuthorizationPolicy)]
    public async Task<IActionResult> CreateNotification([FromBody] CreateNotificationCommand notification)
    {
        var output = await _mediator.Send(notification);
        
        return Ok(output);
    }

    [HttpDelete("{id:guid}")]
    [ProducesResponseType(StatusCodes.Status204NoContent)]
    [Authorize(Policy = Constants.AdminAuthorizationPolicy)]
    public async Task<IActionResult> DeleteNotification(Guid id)
    {
        await _mediator.Send(new DeleteNotificationCommand(new Notification
        {
            Id = id
        }));

        return NoContent();
    }
    
    [HttpGet]
    [ProducesResponseType(typeof(IEnumerable<Notification>), StatusCodes.Status200OK)]
    public async Task<IActionResult> GetAllNotifications([FromQuery] bool includeRead = false)
    {
        var output = await _mediator.Send(new GetAllNotificationsForCurrentUserQuery(includeRead));
        
        return Ok(output);
    }
    
    [HttpGet("unread")]
    [ProducesResponseType(typeof(int), StatusCodes.Status200OK)]
    public async Task<IActionResult> GetUnreadNotificationsCount()
    {
        var output = await _mediator.Send(new GetUnreadNotificationCountQuery());
        
        return Ok(output);
    }

    [HttpGet("all")]
    [ProducesResponseType(typeof(IEnumerable<Notification>), StatusCodes.Status200OK)]
    [Authorize(Policy = Constants.AdminAuthorizationPolicy)]
    public async Task<IActionResult> GetAllNotificationsAdmin()
    {
        var output = await _mediator.Send(new GetAllNotificationsQuery());
        
        return Ok(output);
    }

    [HttpGet("{id:guid}/read-by")]
    [ProducesResponseType(typeof(IEnumerable<RegisteredUser>), StatusCodes.Status200OK)]
    [Authorize(Policy = Constants.AdminAuthorizationPolicy)]
    public async Task<IActionResult> GetAllWhoReadNotification(Guid id)
    {
        var output = await _mediator.Send(new GetAllWhoReadNotificationQuery(new Notification
        {
            Id = id
        }));
        
        return Ok(output);
    }

    [HttpPut("{id:guid}/read")]
    [ProducesResponseType(StatusCodes.Status204NoContent)]
    public async Task<IActionResult> ReadNotification(Guid id)
    {
        await _mediator.Send(new ReadNotificationCommand(new Notification
        {
            Id = id
        }));
        
        return NoContent();
    }
}