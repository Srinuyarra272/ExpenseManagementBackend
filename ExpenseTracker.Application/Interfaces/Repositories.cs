using ExpenseTracker.Domain.Entities;

namespace ExpenseTracker.Application.Interfaces;

public interface ICategoryRepository
{
    Task<IEnumerable<Category>> GetAllAsync();
    Task<Category?> GetByIdAsync(string id);
    Task<IEnumerable<Category>> GetByIdsAsync(IEnumerable<string> ids);
    Task AddAsync(Category category);
    Task DeleteAsync(string id);
}

public interface IBudgetRepository
{
    Task<IEnumerable<Budget>> GetByMonthAsync(string userId, int month, int year);
    Task<Budget?> GetAsync(string id);
    Task<decimal> GetTotalBudgetAsync(string userId, int month, int year);
    Task AddAsync(Budget budget);
    Task UpdateAsync(Budget budget);
    Task DeleteAsync(string id);
}

public interface IBillRepository
{
    Task<IEnumerable<Bill>> GetAllAsync(string userId);
    Task<IEnumerable<Bill>> GetUpcomingAsync(string userId, DateTime fromDate, DateTime toDate);
    Task<Bill?> GetAsync(string id);
    Task AddAsync(Bill bill);
    Task UpdateAsync(Bill bill);
    Task DeleteAsync(string id);
}
