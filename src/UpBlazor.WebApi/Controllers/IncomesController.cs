using MediatR;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using UpBlazor.Application.Features.Incomes;
using UpBlazor.Core.Models;

namespace UpBlazor.WebApi.Controllers;

[ApiController]
[Route("api/[controller]")]
[Authorize]
public class IncomesController : Controller
{
    private readonly IMediator _mediator;
    
    public IncomesController(IMediator mediator) => _mediator = mediator;

    [HttpPost]
    [ProducesResponseType(typeof(Guid), StatusCodes.Status200OK)]
    public async Task<IActionResult> CreateIncome([FromBody] CreateIncomeCommand income)
    {
        var output = await _mediator.Send(income);

        return Ok(output);
    }

    [HttpPut("{id:guid}")]
    [ProducesResponseType(StatusCodes.Status204NoContent)]
    public async Task<IActionResult> UpdateIncome(Guid id, [FromBody] Income income)
    {
        income.Id = id;
        await _mediator.Send(new UpdateIncomeCommand(income));

        return NoContent();
    }
    
    [HttpDelete("{id:guid}")]
    [ProducesResponseType(StatusCodes.Status204NoContent)]
    public async Task<IActionResult> DeleteIncome(Guid id)
    {
        await _mediator.Send(new DeleteIncomeCommand(new Income
        {
            Id = id
        }));

        return NoContent();
    }
    
    [HttpGet]
    [ProducesResponseType(typeof(IEnumerable<Income>), StatusCodes.Status200OK)]
    public async Task<IActionResult> GetIncomes()
    {
        var output = await _mediator.Send(new GetIncomesQuery());

        return Ok(output);
    }
}