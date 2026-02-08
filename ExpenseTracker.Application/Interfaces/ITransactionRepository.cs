using ExpenseTracker.Application.DTOs;
using ExpenseTracker.Domain.Entities;
using ExpenseTracker.Domain.Enums;

namespace ExpenseTracker.Application.Interfaces;

public interface ITransactionRepository
{
    Task<IEnumerable<Transaction>> GetAllAsync(string userId);
    Task<Transaction?> GetByIdAsync(string id, string userId);
    Task AddAsync(Transaction transaction);
    Task UpdateAsync(Transaction transaction);
    Task DeleteAsync(string id, string userId);
    
    // Dashboard specific queries
    Task<decimal> GetTotalSpentAsync(string userId, DateTime startDate, DateTime endDate);
    Task<decimal> GetTotalAmountAsync(string userId, DateTime startDate, DateTime endDate, TransactionType? type);
    Task<IEnumerable<CategoryStats>> GetTopCategoriesAsync(string userId, DateTime startDate, DateTime endDate, int count);
    Task<int> GetTransactionCountAsync(string userId, DateTime startDate, DateTime endDate);
    Task<IEnumerable<Transaction>> GetRecentTransactionsAsync(string userId, int count);
    Task<IEnumerable<Transaction>> GetRecentTransactionsAsync(string userId, DateTime startDate, DateTime endDate, int count);
    Task<IEnumerable<Transaction>> GetByMonthAsync(string userId, int month, int year);
    
    // Search & Filter
    Task<PagedResult<Transaction>> GetFilteredAsync(string userId, TransactionFilterParams filterParams);
    Task<IEnumerable<DailySpendingStat>> GetDailySpendingAsync(string userId, DateTime startDate, DateTime endDate);
}

public class DailySpendingStat
{
    public DateTime Date { get; set; }
    public decimal TotalAmount { get; set; }
}

public class CategoryStats
{
    public string CategoryName { get; set; } = string.Empty;
    public decimal TotalAmount { get; set; }
    public string Color { get; set; } = string.Empty;
}
