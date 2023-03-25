using MediatR;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using UpBlazor.Application.Features.Expenses;
using UpBlazor.Domain.Models;

namespace UpBlazor.WebApi.Controllers;

[ApiController]
[Route("api/[controller]")]
[Authorize]
public class ExpensesController : Controller
{
    private readonly IMediator _mediator;

    public ExpensesController(IMediator mediator) => _mediator = mediator;

    [HttpPost]
    [ProducesResponseType(typeof(Guid), StatusCodes.Status200OK)]
    public async Task<IActionResult> CreateExpense([FromBody] CreateExpenseCommand expense)
    {
        var output = await _mediator.Send(expense);

        return Ok(output);
    }
    
    [HttpDelete("{id:guid}")]
    [ProducesResponseType(StatusCodes.Status204NoContent)]
    public async Task<IActionResult> DeleteExpense(Guid id)
    {
        await _mediator.Send(new DeleteExpenseCommand(new Expense
        {
            Id = id
        }));

        return NoContent();
    }
    
    [HttpGet]
    [ProducesResponseType(typeof(IEnumerable<Expense>), StatusCodes.Status200OK)]
    public async Task<IActionResult> GetExpenses()
    {
        var output = await _mediator.Send(new GetExpensesQuery());

        return Ok(output);
    }
    
    [HttpPut("{id:guid}")]
    [ProducesResponseType(StatusCodes.Status204NoContent)]
    public async Task<IActionResult> UpdateExpense(Guid id, [FromBody] Expense expense)
    {
        expense.Id = id;
        await _mediator.Send(new UpdateExpenseCommand(expense));

        return NoContent();
    }
}