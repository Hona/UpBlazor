using MediatR;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using UpBlazor.Application.Features.Forecast;

namespace UpBlazor.WebApi.Controllers.Features;

[ApiController]
[Route("api/[controller]")]
[Authorize]
public class ForecastController : Controller
{
    private readonly IMediator _mediator;
    
    public ForecastController(IMediator mediator) => _mediator = mediator;

    [HttpGet("expenses")]
    [ProducesResponseType(typeof(IEnumerable<ForecastDto>), StatusCodes.Status200OK)]
    public async Task<IActionResult> GetExpenseForecast([FromQuery] int totalDays)
    {
        var output = await _mediator.Send(new GetExpenseForecastQuery(totalDays));
        
        return Ok(output);
    }
    
    [HttpGet("income")]
    [ProducesResponseType(typeof(IEnumerable<ForecastDto>), StatusCodes.Status200OK)]
    public async Task<IActionResult> GetIncomeForecast([FromQuery] int totalDays)
    {
        var output = await _mediator.Send(new GetIncomeForecastQuery(totalDays));
        
        return Ok(output);
    }
    
    [HttpGet("totals")]
    [ProducesResponseType(typeof(IEnumerable<ForecastDto>), StatusCodes.Status200OK)]
    [ProducesResponseType(StatusCodes.Status403Forbidden)]
    public async Task<IActionResult> GetTotalsForecast([FromQuery] int totalDays)
    {
        var output = await _mediator.Send(new GetTotalForecastQuery(totalDays));
        
        return Ok(output);
    }
}