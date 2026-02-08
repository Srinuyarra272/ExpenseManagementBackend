using ExpenseTracker.Application.Features.Dashboard.Queries;
using MediatR;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace ExpenseTracker.API.Controllers;

[Authorize]
[ApiController]
[Route("api/[controller]")]
public class DashboardController : ControllerBase
{
    private readonly IMediator _mediator;

    public DashboardController(IMediator mediator)
    {
        _mediator = mediator;
    }

    [HttpGet]
    public async Task<IActionResult> GetDashboardMetrics([FromQuery] int? month, [FromQuery] int? year)
    {
        var query = new GetDashboardMetricsQuery { Month = month, Year = year };
        var result = await _mediator.Send(query);
        return Ok(result);
    }
}
