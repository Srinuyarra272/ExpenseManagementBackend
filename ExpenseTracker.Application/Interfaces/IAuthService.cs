using ExpenseTracker.Application.DTOs.Auth;

namespace ExpenseTracker.Application.Interfaces;

public interface IAuthService
{
    Task<AuthResponseDto> RegisterAsync(RegisterDto request);
    Task<AuthResponseDto> LoginAsync(LoginDto request);
}
