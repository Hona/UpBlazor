using MediatR;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using UpBlazor.Application.Features.SavingsPlans;
using UpBlazor.Domain.Common.Exceptions;
using UpBlazor.Domain.Entities;

namespace UpBlazor.WebApi.Controllers;

[ApiController]
[Route("api/[controller]")]
[Authorize]
public class SavingsPlanController : Controller
{
    private readonly IMediator _mediator;
    
    public SavingsPlanController(IMediator mediator) => _mediator = mediator;
    
    [HttpPost]
    [ProducesResponseType(typeof(Guid), StatusCodes.Status200OK)]
    public async Task<IActionResult> CreateSavingsPlan([FromBody] CreateSavingsPlanCommand command)
    {
        var result = await _mediator.Send(command);
        
        return Ok(result);
    }
    
    [HttpDelete("{id:guid}")]
    [ProducesResponseType(StatusCodes.Status204NoContent)]
    public async Task<IActionResult> DeleteSavingsPlan(Guid id)
    {
        await _mediator.Send(new DeleteSavingsPlanCommand(new SavingsPlan
        {
            Id = id
        }));
        
        return NoContent();
    }
    
    [HttpGet("/api/income/{incomeId:guid}/savings-plans")]
    [ProducesResponseType(typeof(IEnumerable<SavingsPlan>), StatusCodes.Status200OK)]
    [ProducesResponseType(typeof(BadRequestException), StatusCodes.Status400BadRequest)]
    public async Task<IActionResult> GetSavingsPlans(Guid incomeId)
    {
        var result = await _mediator.Send(new GetSavingsPlansQuery(incomeId));
        
        return Ok(result);
    }
    
    [HttpPut("{id:guid}")]
    [ProducesResponseType(StatusCodes.Status204NoContent)]
    public async Task<IActionResult> UpdateSavingsPlan(Guid id, [FromBody] SavingsPlan savingsPlan)
    {
        savingsPlan.Id = id;
        await _mediator.Send(new UpdateSavingsPlanCommand(savingsPlan));
        
        return NoContent();
    }
}