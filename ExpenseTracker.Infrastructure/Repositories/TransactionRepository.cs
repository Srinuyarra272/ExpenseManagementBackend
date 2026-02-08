using ExpenseTracker.Application.DTOs;
using ExpenseTracker.Application.Interfaces;
using ExpenseTracker.Domain.Entities;
using ExpenseTracker.Domain.Enums;
using ExpenseTracker.Infrastructure.Data;
using MongoDB.Driver;

namespace ExpenseTracker.Infrastructure.Repositories;

public class TransactionRepository : ITransactionRepository
{
    private readonly ExpenseTrackerContext _context;

    public TransactionRepository(ExpenseTrackerContext context)
    {
        _context = context;
    }

    public async Task<IEnumerable<Transaction>> GetAllAsync(string userId)
    {
        return await _context.Transactions.Find(t => t.UserId == userId).ToListAsync();
    }

    public async Task<Transaction?> GetByIdAsync(string id, string userId)
    {
        return await _context.Transactions.Find(t => t.Id == id && t.UserId == userId).FirstOrDefaultAsync();
    }

    public async Task AddAsync(Transaction transaction)
    {
        await _context.Transactions.InsertOneAsync(transaction);
    }

    public async Task UpdateAsync(Transaction transaction)
    {
        await _context.Transactions.ReplaceOneAsync(t => t.Id == transaction.Id, transaction);
    }

    public async Task DeleteAsync(string id, string userId)
    {
        await _context.Transactions.DeleteOneAsync(t => t.Id == id && t.UserId == userId);
    }

    public async Task<decimal> GetTotalSpentAsync(string userId, DateTime startDate, DateTime endDate)
    {
        return await GetTotalAmountAsync(userId, startDate, endDate, TransactionType.Expense);
    }

    public async Task<decimal> GetTotalAmountAsync(string userId, DateTime startDate, DateTime endDate, TransactionType? type)
    {
        var builder = Builders<Transaction>.Filter;
        var filter = builder.And(
            builder.Eq(t => t.UserId, userId),
            builder.Gte(t => t.Date, startDate),
            builder.Lte(t => t.Date, endDate)
        );

        if (type.HasValue)
        {
            filter &= builder.Eq(t => t.Type, type.Value);
        }

        var transactions = await _context.Transactions.Find(filter).ToListAsync();
        return transactions.Sum(t => t.Amount);
    }

    public async Task<IEnumerable<CategoryStats>> GetTopCategoriesAsync(string userId, DateTime startDate, DateTime endDate, int count)
    {
        // This requires an aggregation pipeline joining with Categories if names are needed from Category collection
        // Assuming CategoryId is stored, we might need a lookup.
        // For simplicity, let's fetch and group in memory if dataset is small, or better use Aggregate.
        // Let's use Aggregate.
        
        // Removed unused pipeline definition

            
        // Note: To get Category Name and Color, we need $lookup. 
        // Proceeding with basic aggregation first. 
        // We will need a proper DTO to project.
        
        // Let's utilize the $lookup in a strongly typed way if possible, or just fetch categories separately for mapping.
        // Fetching aggregate results:
        
        var results = await _context.Transactions.Aggregate()
            .Match(t => t.UserId == userId && t.Date >= startDate && t.Date <= endDate && t.Type == TransactionType.Expense)
            .Group(t => t.CategoryId, g => new { Key = g.Key, Total = g.Sum(t => t.Amount) })
            .SortByDescending(x => x.Total)
            .Limit(count)
            .ToListAsync();

        // We need to map CategoryId to Name/Color. ID list:
        var categoryIds = results.Select(r => r.Key).ToList();
        var categories = await _context.Categories.Find(c => categoryIds.Contains(c.Id)).ToListAsync();
        
        return results.Select(r => 
        {
            var cat = categories.FirstOrDefault(c => c.Id == r.Key);
            return new CategoryStats
            {
                CategoryName = cat?.Name ?? "Unknown",
                TotalAmount = r.Total,
                Color = cat?.Color ?? "#000000"
            };
        });
    }

    public async Task<int> GetTransactionCountAsync(string userId, DateTime startDate, DateTime endDate)
    {
          var filter = Builders<Transaction>.Filter.And(
            Builders<Transaction>.Filter.Eq(t => t.UserId, userId),
            Builders<Transaction>.Filter.Gte(t => t.Date, startDate),
            Builders<Transaction>.Filter.Lte(t => t.Date, endDate)
        );
        return (int)await _context.Transactions.CountDocumentsAsync(filter);
    }

    public async Task<IEnumerable<Transaction>> GetRecentTransactionsAsync(string userId, int count)
    {
        return await _context.Transactions.Find(t => t.UserId == userId)
            .SortByDescending(t => t.Date)
            .Limit(count)
            .ToListAsync();
    }

    public async Task<IEnumerable<Transaction>> GetRecentTransactionsAsync(string userId, DateTime startDate, DateTime endDate, int count)
    {
        return await _context.Transactions.Find(t => t.UserId == userId && t.Date >= startDate && t.Date <= endDate)
            .SortByDescending(t => t.Date)
            .Limit(count)
            .ToListAsync();
    }

    public async Task<PagedResult<Transaction>> GetFilteredAsync(string userId, TransactionFilterParams filterParams)
    {
        var builder = Builders<Transaction>.Filter;
        var filter = builder.Eq(t => t.UserId, userId);

        if (!string.IsNullOrEmpty(filterParams.SearchText))
        {
            filter &= builder.Regex(t => t.Description, new MongoDB.Bson.BsonRegularExpression(filterParams.SearchText, "i")) |
                      builder.Regex(t => t.Merchant, new MongoDB.Bson.BsonRegularExpression(filterParams.SearchText, "i"));
        }

        if (filterParams.StartDate.HasValue)
        {
            filter &= builder.Gte(t => t.Date, filterParams.StartDate.Value);
        }

        if (filterParams.EndDate.HasValue)
        {
            filter &= builder.Lte(t => t.Date, filterParams.EndDate.Value);
        }

        if (filterParams.CategoryIds != null && filterParams.CategoryIds.Any())
        {
            filter &= builder.In(t => t.CategoryId, filterParams.CategoryIds);
        }

        if (filterParams.MinAmount.HasValue)
        {
            filter &= builder.Gte(t => t.Amount, filterParams.MinAmount.Value);
        }
        
        if (filterParams.MaxAmount.HasValue)
        {
            filter &= builder.Lte(t => t.Amount, filterParams.MaxAmount.Value);
        }

        if (filterParams.Type.HasValue)
        {
            filter &= builder.Eq(t => t.Type, filterParams.Type.Value);
        }

        var totalCount = await _context.Transactions.CountDocumentsAsync(filter);
        var items = await _context.Transactions.Find(filter)
            .SortByDescending(t => t.Date)
            .Skip((filterParams.PageNumber - 1) * filterParams.PageSize)
            .Limit(filterParams.PageSize)
            .ToListAsync();

        return new PagedResult<Transaction>
        {
            Items = items,
            TotalCount = (int)totalCount,
            PageNumber = filterParams.PageNumber,
            PageSize = filterParams.PageSize
        };
    }
    public async Task<IEnumerable<Transaction>> GetByMonthAsync(string userId, int month, int year)
    {
        // Construct date range for the month
        // We assume dates are stored as UTC in DB. 
        // Using explicit range check is better than .Month/Year properties in LINQ for Mongo usually.
        var startDate = new DateTime(year, month, 1);
        var endDate = startDate.AddMonths(1).AddTicks(-1);

        return await _context.Transactions.Find(t => t.UserId == userId && t.Date >= startDate && t.Date <= endDate).ToListAsync();
    }

    public async Task<IEnumerable<DailySpendingStat>> GetDailySpendingAsync(string userId, DateTime startDate, DateTime endDate)
    {
        // Retrieve transactions for the period (in-memory grouping is simpler for Date truncation with MongoDB driver quirks on specific versions)
        var transactions = await _context.Transactions
            .Find(t => t.UserId == userId && t.Date >= startDate && t.Date <= endDate && t.Type == TransactionType.Expense)
            .ToListAsync();
            
        return transactions
            .GroupBy(t => t.Date.Date)
            .Select(g => new DailySpendingStat
            {
                Date = g.Key,
                TotalAmount = g.Sum(t => t.Amount)
            })
            .OrderBy(x => x.Date);
    }
}
