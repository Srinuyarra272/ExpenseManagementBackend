using ExpenseTracker.Application.DTOs.User;
using ExpenseTracker.Application.Features.Users.Commands;
using ExpenseTracker.Application.Features.Users.Queries;
using MediatR;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace ExpenseTracker.API.Controllers;

[Authorize]
[ApiController]
[Route("api/[controller]")]
public class UsersController : ControllerBase
{
    private readonly IMediator _mediator;

    public UsersController(IMediator mediator)
    {
        _mediator = mediator;
    }

    [HttpGet("profile")]
    public async Task<ActionResult<UserProfileDto>> GetProfile()
    {
        var query = new GetUserProfileQuery();
        var result = await _mediator.Send(query);
        return Ok(result);
    }

    [HttpPut("profile")]
    public async Task<ActionResult<UserProfileDto>> UpdateProfile([FromBody] UpdateUserProfileDto dto)
    {
        var command = new UpdateUserProfileCommand { Dto = dto };
        var result = await _mediator.Send(command);
        return Ok(result);
    }

    [HttpPost("change-password")]
    public async Task<IActionResult> ChangePassword([FromBody] ChangePasswordDto dto)
    {
        try
        {
            var command = new ChangePasswordCommand { Dto = dto };
            await _mediator.Send(command);
            return Ok(new { message = "Password updated successfully" });
        }
        catch (Exception ex)
        {
            return BadRequest(new { message = ex.Message });
        }
    }
}
