using MediatR;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using UpBlazor.Application.Features.Planner;
using UpBlazor.Core.Models;

namespace UpBlazor.WebApi.Controllers.Features;

[ApiController]
[Route("api/[controller]")]
[Authorize]
public class PlannerController : Controller
{
    private readonly IMediator _mediator;

    public PlannerController(IMediator mediator) => _mediator = mediator;

    [HttpGet("income/{id:guid}")]
    [ProducesResponseType(typeof(IncomePlannerDto), StatusCodes.Status200OK)]
    [ProducesResponseType(StatusCodes.Status400BadRequest)]
    public async Task<IActionResult> GetIncomePlanner(Guid id, [FromQuery] bool onlyUseSavingsPlans = false)
    {
        var output = await _mediator.Send(new GetIncomePlannerQuery(id, onlyUseSavingsPlans));

        return Ok(output);
    }
}