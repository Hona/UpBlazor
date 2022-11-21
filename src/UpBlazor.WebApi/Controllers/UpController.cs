using System.Text.Json;
using MediatR;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Up.NET.Api.Accounts;
using Up.NET.Api.Transactions;
using Up.NET.Api.Utilities;
using UpBlazor.Application.Features.Up;
using UpBlazor.Application.Services;
using UpBlazor.Core.Exceptions;
using UpBlazor.WebApi.ViewModels;

namespace UpBlazor.WebApi.Controllers;

[ApiController]
[Route("api/[controller]")]
[Authorize]
public class UpController : Controller
{
    private readonly IMediator _mediator;
    private readonly ICurrentUserService _currentUserService;
    
    public UpController(IMediator mediator, ICurrentUserService currentUserService)
    {
        _mediator = mediator;
        _currentUserService = currentUserService;
    }

    [HttpDelete("token")]
    [ProducesResponseType(StatusCodes.Status204NoContent)]
    public async Task<IActionResult> ClearToken()
    {
        await _mediator.Send(new ClearUpAccessTokenCommand());

        return NoContent();
    }

    [HttpPut("token")]
    [ProducesResponseType(StatusCodes.Status204NoContent)]
    [ProducesResponseType(StatusCodes.Status400BadRequest)]
    public async Task<IActionResult> TrySetToken([FromBody] string accessToken)
    {
        await _mediator.Send(new TrySetUpAccessTokenCommand(accessToken));

        return NoContent();
    }

    [HttpGet("ping")]
    [ProducesResponseType(typeof(PingResponse), StatusCodes.Status200OK)]
    [ProducesResponseType(StatusCodes.Status400BadRequest)]
    public async Task<IActionResult> Ping([FromQuery] bool forceReload = false)
    {
        var output = await _mediator.Send(new GetUpPingQuery(forceReload));

        if (!output.Success)
        {
            return BadRequest();
        }

        return Ok(output.Response);
    }

    [HttpGet("accounts")]
    [ProducesResponseType(typeof(List<AccountResource>), StatusCodes.Status200OK)]
    public async Task<IActionResult> GetAccounts()
    {
        var output = await _mediator.Send(new GetUpAccountsQuery());

        return Ok(output);
    }

    [HttpGet("accounts/{accountId}/transactions")]
    [ProducesResponseType(typeof(UpPaginatedViewModel<TransactionResource>), StatusCodes.Status200OK)]
    public async Task<IActionResult> GetTransactions(string accountId)
    {
        var output = await _mediator.Send(new GetUpTransactionsPagedQuery(accountId));

        if (!output.Success)
        {
            throw new BadRequestException(JsonSerializer.Serialize(output.Errors));
        }
        
        return Ok(new UpPaginatedViewModel<TransactionResource>
        {
            Data = output.Response.Data,
            Links = output.Response.Links
        });
    }
    
    [HttpGet("accounts/transactions")]
    [ProducesResponseType(typeof(UpPaginatedViewModel<TransactionResource>), StatusCodes.Status200OK)]
    public async Task<IActionResult> GetAllTransactions()
    {
        var output = await _mediator.Send(new GetUpTransactionsPagedQuery());

        if (!output.Success)
        {
            throw new BadRequestException(JsonSerializer.Serialize(output.Errors));
        }
        
        return Ok(new UpPaginatedViewModel<TransactionResource>
        {
            Data = output.Response.Data,
            Links = output.Response.Links
        });
    }
    
    
    [HttpGet("transaction/page")]
    [ProducesResponseType(typeof(UpPaginatedViewModel<TransactionResource>), StatusCodes.Status200OK)]
    public async Task<IActionResult> GetTransactionsPaginationLink([FromQuery] string link)
    {
        var upApi = await _currentUserService.GetApiAsync();

        var output = await upApi.SendPaginatedRequestAsync<TransactionResource>(HttpMethod.Get, link, urlIsAbsolute: true);

        if (!output.Success)
        {
            throw new BadRequestException(JsonSerializer.Serialize(output.Errors));
        }
        
        return Ok(new UpPaginatedViewModel<TransactionResource>
        {
            Data = output.Response.Data,
            Links = output.Response.Links
        });
    }
}