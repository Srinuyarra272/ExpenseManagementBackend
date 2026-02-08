using ExpenseTracker.Application.DTOs.User;
using ExpenseTracker.Application.Features.Users.Commands;
using ExpenseTracker.Application.Features.Users.Queries;
using ExpenseTracker.Application.Interfaces;
using MediatR;

namespace ExpenseTracker.Application.Features.Users.Handlers;

public class UserHandlers : 
    IRequestHandler<GetUserProfileQuery, UserProfileDto>,
    IRequestHandler<UpdateUserProfileCommand, UserProfileDto>,
    IRequestHandler<ChangePasswordCommand, bool>
{
    private readonly IUserRepository _userRepository;
    private readonly ICurrentUserService _currentUserService;
    private readonly IPasswordHasher _passwordHasher;

    public UserHandlers(
        IUserRepository userRepository, 
        ICurrentUserService currentUserService,
        IPasswordHasher passwordHasher)
    {
        _userRepository = userRepository;
        _currentUserService = currentUserService;
        _passwordHasher = passwordHasher;
    }

    public async Task<UserProfileDto> Handle(GetUserProfileQuery request, CancellationToken cancellationToken)
    {
        var userId = _currentUserService.UserId ?? throw new UnauthorizedAccessException();
        var user = await _userRepository.GetByIdAsync(userId);
        if (user == null) throw new Exception("User not found");

        return new UserProfileDto
        {
            Id = user.Id,
            FirstName = user.FirstName,
            LastName = user.LastName,
            Email = user.Email
        };
    }

    public async Task<UserProfileDto> Handle(UpdateUserProfileCommand request, CancellationToken cancellationToken)
    {
        var userId = _currentUserService.UserId ?? throw new UnauthorizedAccessException();
        var user = await _userRepository.GetByIdAsync(userId);
        if (user == null) throw new Exception("User not found");

        user.FirstName = request.Dto.FirstName;
        user.LastName = request.Dto.LastName;
        user.Email = request.Dto.Email;

        await _userRepository.UpdateAsync(user);

        return new UserProfileDto
        {
            Id = user.Id,
            FirstName = user.FirstName,
            LastName = user.LastName,
            Email = user.Email
        };
    }

    public async Task<bool> Handle(ChangePasswordCommand request, CancellationToken cancellationToken)
    {
        var userId = _currentUserService.UserId ?? throw new UnauthorizedAccessException();
        var user = await _userRepository.GetByIdAsync(userId);
        if (user == null) throw new Exception("User not found");

        if (!_passwordHasher.VerifyPassword(request.Dto.CurrentPassword, user.PasswordHash))
        {
            throw new Exception("Invalid current password");
        }

        user.PasswordHash = _passwordHasher.HashPassword(request.Dto.NewPassword);
        await _userRepository.UpdateAsync(user);

        return true;
    }
}
