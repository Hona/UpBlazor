using MediatR;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using UpBlazor.Application.Features.Goals;
using UpBlazor.Core.Models;

namespace UpBlazor.WebApi.Controllers;

[ApiController]
[Route("api/[controller]")]
[Authorize]
public class GoalsController : Controller
{
    private readonly IMediator _mediator;
    
    public GoalsController(IMediator mediator) => _mediator = mediator;
    
    [HttpGet]
    [ProducesResponseType(typeof(IEnumerable<Goal>), StatusCodes.Status200OK)]
    public async Task<IActionResult> GetGoals()
    {
        var output = await _mediator.Send(new GetGoalsQuery());
        
        return Ok(output);
    }
    
    [HttpDelete("{id:guid}")]
    [ProducesResponseType(StatusCodes.Status204NoContent)]
    public async Task<IActionResult> DeleteGoal(Guid id)
    {
        await _mediator.Send(new DeleteGoalCommand(new Goal
        {
            Id = id
        }));
        
        return NoContent();
    }
    
    [HttpPost]
    [ProducesResponseType(typeof(Guid), StatusCodes.Status200OK)]
    public async Task<IActionResult> CreateGoal([FromBody] CreateGoalCommand goal)
    {
        var output = await _mediator.Send(goal);
        
        return Ok(output);
    }
}