using ExpenseTracker.Application.DTOs;
using ExpenseTracker.Application.Features.Budgets.Commands;
using ExpenseTracker.Application.Features.Budgets.Queries;
using MediatR;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace ExpenseTracker.API.Controllers;

[Authorize]
[ApiController]
[Route("api/[controller]")]
public class BudgetsController : ControllerBase
{
    private readonly IMediator _mediator;

    public BudgetsController(IMediator mediator)
    {
        _mediator = mediator;
    }

    [HttpGet]
    public async Task<ActionResult<List<BudgetDto>>> GetBudgets([FromQuery] int month, [FromQuery] int year)
    {
        if (month == 0 || year == 0)
        {
            var now = DateTime.UtcNow;
            month = now.Month;
            year = now.Year;
        }

        var query = new GetBudgetsQuery(month, year);
        var result = await _mediator.Send(query);
        return Ok(result);
    }

    [HttpPost]
    public async Task<ActionResult<string>> CreateBudget(CreateBudgetDto dto)
    {
        var command = new CreateBudgetCommand(dto);
        var id = await _mediator.Send(command);
        return CreatedAtAction(nameof(GetBudgets), new { id }, id);
    }

    [HttpPut("{id}")]
    public async Task<ActionResult> UpdateBudget(string id, [FromBody] decimal amount)
    {
        var command = new UpdateBudgetCommand(id, amount);
        await _mediator.Send(command);
        return NoContent();
    }

    [HttpDelete("{id}")]
    public async Task<ActionResult> DeleteBudget(string id)
    {
        var command = new DeleteBudgetCommand(id);
        await _mediator.Send(command);
        return NoContent();
    }
}
