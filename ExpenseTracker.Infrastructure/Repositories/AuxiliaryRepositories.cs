using ExpenseTracker.Application.Interfaces;
using ExpenseTracker.Domain.Entities;
using ExpenseTracker.Infrastructure.Data;
using MongoDB.Driver;

namespace ExpenseTracker.Infrastructure.Repositories;

public class CategoryRepository : ICategoryRepository
{
    private readonly ExpenseTrackerContext _context;

    public CategoryRepository(ExpenseTrackerContext context)
    {
        _context = context;
    }

    public async Task<IEnumerable<Category>> GetAllAsync()
    {
        return await _context.Categories.Find(_ => true).ToListAsync();
    }

    public async Task<Category?> GetByIdAsync(string id)
    {
        return await _context.Categories.Find(c => c.Id == id).FirstOrDefaultAsync();
    }

    public async Task<IEnumerable<Category>> GetByIdsAsync(IEnumerable<string> ids)
    {
        var filter = Builders<Category>.Filter.In(c => c.Id, ids);
        return await _context.Categories.Find(filter).ToListAsync();
    }

    public async Task AddAsync(Category category)
    {
        await _context.Categories.InsertOneAsync(category);
    }

    public async Task DeleteAsync(string id)
    {
        await _context.Categories.DeleteOneAsync(c => c.Id == id);
    }
}

public class BudgetRepository : IBudgetRepository
{
    private readonly ExpenseTrackerContext _context;

    public BudgetRepository(ExpenseTrackerContext context)
    {
        _context = context;
    }

    public async Task<IEnumerable<Budget>> GetByMonthAsync(string userId, int month, int year)
    {
        return await _context.Budgets.Find(b => b.UserId == userId && b.Month == month && b.Year == year).ToListAsync();
    }

    public async Task<Budget?> GetAsync(string id)
    {
         return await _context.Budgets.Find(b => b.Id == id).FirstOrDefaultAsync();
    }

    public async Task<decimal> GetTotalBudgetAsync(string userId, int month, int year)
    {
        var budgets = await GetByMonthAsync(userId, month, year);
        return budgets.Sum(b => b.Amount);
    }

    public async Task AddAsync(Budget budget)
    {
        await _context.Budgets.InsertOneAsync(budget);
    }

    public async Task UpdateAsync(Budget budget)
    {
        await _context.Budgets.ReplaceOneAsync(b => b.Id == budget.Id, budget);
    }

    public async Task DeleteAsync(string id)
    {
        await _context.Budgets.DeleteOneAsync(b => b.Id == id);
    }
}
