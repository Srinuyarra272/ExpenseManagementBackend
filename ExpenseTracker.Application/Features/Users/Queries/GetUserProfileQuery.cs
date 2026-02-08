using ExpenseTracker.Application.DTOs.User;
using MediatR;

namespace ExpenseTracker.Application.Features.Users.Queries;

public class GetUserProfileQuery : IRequest<UserProfileDto>
{
}
