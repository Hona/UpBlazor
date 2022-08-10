using MediatR;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using UpBlazor.Application.Features.Users;

namespace UpBlazor.WebApi.Controllers.Features;

[ApiController]
[Route("api/[controller]")]
[Authorize]
public class UsersController : Controller
{
    private readonly IMediator _mediator;

    public UsersController(IMediator mediator) => _mediator = mediator;

    [HttpGet]
    [ProducesResponseType(typeof(IEnumerable<RegisteredUserDto>), StatusCodes.Status200OK)]
    [Authorize(Policy = Constants.AdminAuthorizationPolicy)]
    public async Task<IActionResult> GetAll()
    {
        var output = await _mediator.Send(new GetAllUsersQuery());
        
        return Ok(output);
    }
}