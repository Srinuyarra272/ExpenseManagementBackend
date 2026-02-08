using ExpenseTracker.Domain.Entities;

namespace ExpenseTracker.Application.Interfaces;

public interface IUserRepository
{
    Task<User?> GetByIdAsync(string id);
    Task UpdateAsync(User user);
}
