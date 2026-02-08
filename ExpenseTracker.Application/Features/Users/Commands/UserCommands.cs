using ExpenseTracker.Application.DTOs.User;
using MediatR;

namespace ExpenseTracker.Application.Features.Users.Commands;

public class UpdateUserProfileCommand : IRequest<UserProfileDto>
{
    public UpdateUserProfileDto Dto { get; set; } = new();
}

public class ChangePasswordCommand : IRequest<bool>
{
    public ChangePasswordDto Dto { get; set; } = new();
}
