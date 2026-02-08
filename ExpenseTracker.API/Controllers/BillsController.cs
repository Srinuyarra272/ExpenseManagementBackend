using ExpenseTracker.Application.Features.Bills.Commands;
using ExpenseTracker.Application.Features.Bills.Queries;
using MediatR;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace ExpenseTracker.API.Controllers;

[Authorize]
[ApiController]
[Route("api/[controller]")]
public class BillsController : ControllerBase
{
    private readonly IMediator _mediator;

    public BillsController(IMediator mediator)
    {
        _mediator = mediator;
    }

    [HttpGet]
    public async Task<IActionResult> GetBills()
    {
        var query = new GetBillsQuery();
        var result = await _mediator.Send(query);
        return Ok(result);
    }

    [HttpPost]
    public async Task<IActionResult> CreateBill([FromBody] CreateBillCommand command)
    {
        var id = await _mediator.Send(command);
        return Ok(new { id });
    }

    [HttpPut("{id}")]
    public async Task<IActionResult> UpdateBill(string id, [FromBody] UpdateBillCommand command)
    {
        command.Id = id;
        await _mediator.Send(command);
        return NoContent();
    }

    [HttpDelete("{id}")]
    public async Task<IActionResult> DeleteBill(string id)
    {
        await _mediator.Send(new DeleteBillCommand { Id = id });
        return NoContent();
    }

    [HttpPatch("{id}/paid")]
    public async Task<IActionResult> MarkAsPaid(string id, [FromBody] MarkBillAsPaidCommand command)
    {
        command.Id = id;
        await _mediator.Send(command);
        return NoContent();
    }
}
