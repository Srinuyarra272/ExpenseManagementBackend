using ExpenseTracker.Application.DTOs;
using ExpenseTracker.Application.Features.Transactions.Commands;
using ExpenseTracker.Application.Features.Transactions.Queries;
using MediatR;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace ExpenseTracker.API.Controllers;

[Authorize]
[ApiController]
[Route("api/[controller]")]
public class TransactionsController : ControllerBase
{
    private readonly IMediator _mediator;

    public TransactionsController(IMediator mediator)
    {
        _mediator = mediator;
    }

    [HttpPost]
    public async Task<IActionResult> CreateTransaction([FromForm] CreateTransactionDto dto)
    {
        var command = new CreateTransactionCommand 
        { 
            Dto = dto
        };
        var id = await _mediator.Send(command);
        return Ok(new { Id = id });
    }

    [HttpGet]
    public async Task<ActionResult<PagedResult<TransactionDto>>> GetTransactions([FromQuery] TransactionFilterParams filterParams)
    {
        var query = new GetTransactionsQuery { FilterParams = filterParams };
        var result = await _mediator.Send(query);
        return Ok(result);
    }

    [HttpPut("{id}")]
    public async Task<IActionResult> UpdateTransaction(string id, [FromForm] CreateTransactionDto dto)
    {
        var command = new UpdateTransactionCommand 
        { 
            Id = id,
            Dto = dto
        };
        await _mediator.Send(command);
        return NoContent();
    }

    [HttpDelete("{id}")]
    public async Task<IActionResult> DeleteTransaction(string id)
    {
        var command = new DeleteTransactionCommand { Id = id };
        await _mediator.Send(command);
        return NoContent();
    }
}
