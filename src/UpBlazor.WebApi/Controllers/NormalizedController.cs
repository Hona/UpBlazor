using MediatR;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using UpBlazor.Application.Features.Normalized;
using UpBlazor.Core.Models;

namespace UpBlazor.WebApi.Controllers;

[ApiController]
[Route("api/[controller]")]
[Authorize]
public class NormalizedController : Controller
{
    private readonly IMediator _mediator;
    
    public NormalizedController(IMediator mediator) => _mediator = mediator;
    
    [HttpGet]
    [ProducesResponseType(typeof(NormalizedAggregate), StatusCodes.Status200OK)]
    public async Task<IActionResult> Get()
    {
        var output = await _mediator.Send(new GetNormalizedAggregateQuery());
        
        return Ok(output);
    }

    [HttpPost("update")]
    [ProducesResponseType(StatusCodes.Status204NoContent)]
    public async Task<IActionResult> Update()
    {
        await _mediator.Send(new UpdateNormalizedAggregateCommand());

        return NoContent();
    }
}