using MediatR;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using UpBlazor.Application.Features.RecurringExpenses;
using UpBlazor.Core.Models;

namespace UpBlazor.WebApi.Controllers;

[ApiController]
[Route("api/expenses/recurring")]
[Authorize]
public class RecurringExpensesController : Controller
{
    private readonly IMediator _mediator;
    
    public RecurringExpensesController(IMediator mediator) => _mediator = mediator;

    [HttpPost]
    [ProducesResponseType(typeof(Guid), StatusCodes.Status200OK)]
    public async Task<IActionResult> CreateRecurringExpense([FromBody] CreateRecurringExpenseCommand expense)
    {
        var output = await _mediator.Send(expense);
        
        return Ok(output);
    }
    
    [HttpDelete("{id:guid}")]
    [ProducesResponseType(StatusCodes.Status204NoContent)]
    public async Task<IActionResult> DeleteRecurringExpense(Guid id)
    {
        await _mediator.Send(new DeleteRecurringExpenseCommand(new RecurringExpense
        {
            Id = id
        }));
        
        return NoContent();
    }
    
    [HttpGet]
    [ProducesResponseType(typeof(IEnumerable<RecurringExpense>), StatusCodes.Status200OK)]
    public async Task<IActionResult> GetRecurringExpenses()
    {
        var output = await _mediator.Send(new GetRecurringExpensesQuery());
        
        return Ok(output);
    }
}