using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Text;
using ExpenseTracker.Application.DTOs.Auth;
using ExpenseTracker.Application.Interfaces;
using ExpenseTracker.Domain.Entities;
using ExpenseTracker.Infrastructure.Data;
using Microsoft.Extensions.Configuration;
using Microsoft.IdentityModel.Tokens;
using MongoDB.Driver;

namespace ExpenseTracker.Infrastructure.Services;

public class AuthService : IAuthService
{
    private readonly ExpenseTrackerContext _context;
    private readonly IConfiguration _configuration;

    public AuthService(ExpenseTrackerContext context, IConfiguration configuration)
    {
        _context = context;
        _configuration = configuration;
    }

    // Helper property to access Users collection
    private IMongoCollection<User> Users => _context.Transactions.Database.GetCollection<User>("Users");

    public async Task<AuthResponseDto> RegisterAsync(RegisterDto request)
    {
        // Check if user exists
        var existingUser = await Users.Find(u => u.Email == request.Email).FirstOrDefaultAsync();
        if (existingUser != null)
        {
            throw new Exception("User with this email already exists.");
        }

        // Hash password (In production use a robust hasher like BCrypt)
        var passwordHash = BCrypt.Net.BCrypt.HashPassword(request.Password);

        var user = new User
        {
            Id = Guid.NewGuid().ToString(),
            FirstName = request.FirstName,
            LastName = request.LastName,
            Email = request.Email,
            PasswordHash = passwordHash,
            Role = "User"
        };

        await Users.InsertOneAsync(user);
        var token = GenerateJwtToken(user);

        return new AuthResponseDto
        {
            Id = user.Id,
            FirstName = user.FirstName,
            LastName = user.LastName,
            Email = user.Email,
            Token = token
        };
    }

    public async Task<AuthResponseDto> LoginAsync(LoginDto request)
    {
        var user = await Users.Find(u => u.Email == request.Email).FirstOrDefaultAsync();
        if (user == null)
        {
            throw new Exception("Invalid email or password.");
        }

        if (!BCrypt.Net.BCrypt.Verify(request.Password, user.PasswordHash))
        {
            throw new Exception("Invalid email or password.");
        }

        var token = GenerateJwtToken(user, request.RememberMe);

        return new AuthResponseDto
        {
            Id = user.Id,
            FirstName = user.FirstName,
            LastName = user.LastName,
            Email = user.Email,
            Token = token
        };
    }

    private string GenerateJwtToken(User user, bool rememberMe = false)
    {
        var jwtSettings = _configuration.GetSection("JwtSettings");
        var key = Encoding.ASCII.GetBytes(jwtSettings["SecretKey"]!);

        var claims = new List<Claim>
        {
            new Claim(ClaimTypes.NameIdentifier, user.Id),
            new Claim(ClaimTypes.Email, user.Email),
            new Claim(ClaimTypes.Role, user.Role),
            new Claim("FirstName", user.FirstName)
        };

        var tokenDescriptor = new SecurityTokenDescriptor
        {
            Subject = new ClaimsIdentity(claims),
            Expires = DateTime.UtcNow.AddDays(rememberMe ? 30 : 1),
            SigningCredentials = new SigningCredentials(new SymmetricSecurityKey(key), SecurityAlgorithms.HmacSha256Signature),
            Issuer = jwtSettings["Issuer"],
            Audience = jwtSettings["Audience"]
        };

        var tokenHandler = new JwtSecurityTokenHandler();
        var token = tokenHandler.CreateToken(tokenDescriptor);
        return tokenHandler.WriteToken(token);
    }
}
