using ExpenseTracker.Application.Interfaces;
using ExpenseTracker.Domain.Entities;
using ExpenseTracker.Infrastructure.Data;
using MongoDB.Driver;

namespace ExpenseTracker.Infrastructure.Repositories;

public class UserRepository : IUserRepository
{
    private readonly ExpenseTrackerContext _context;

    public UserRepository(ExpenseTrackerContext context)
    {
        _context = context;
    }

    private IMongoCollection<User> Users => _context.Transactions.Database.GetCollection<User>("Users");

    public async Task<User?> GetByIdAsync(string id)
    {
        return await Users.Find(u => u.Id == id).FirstOrDefaultAsync();
    }

    public async Task UpdateAsync(User user)
    {
        await Users.ReplaceOneAsync(u => u.Id == user.Id, user);
    }
}
