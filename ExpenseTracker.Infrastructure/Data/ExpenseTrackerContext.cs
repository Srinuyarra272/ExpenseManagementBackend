using ExpenseTracker.Domain.Entities;
using ExpenseTracker.Infrastructure.Settings;
using Microsoft.Extensions.Options;
using MongoDB.Driver;
using ExpenseTracker.Domain.Enums;

namespace ExpenseTracker.Infrastructure.Data;

public class ExpenseTrackerContext
{
    private readonly IMongoDatabase _database;

    public ExpenseTrackerContext(IOptions<MongoDbSettings> settings)
    {
        var client = new MongoClient(settings.Value.ConnectionString);
        _database = client.GetDatabase(settings.Value.DatabaseName);
    }

    public IMongoCollection<Transaction> Transactions => _database.GetCollection<Transaction>("Transactions");
    public IMongoCollection<Category> Categories => _database.GetCollection<Category>("Categories");
    public IMongoCollection<Budget> Budgets => _database.GetCollection<Budget>("Budgets");
    public IMongoCollection<Bill> Bills => _database.GetCollection<Bill>("Bills");

    public async Task SeedDataAsync()
    {
        if (await Categories.CountDocumentsAsync(_ => true) == 0)
        {
            var categories = new List<Category>
            {
                new Category { Id = Guid.NewGuid().ToString(), Name = "Food", Icon = "restaurant", Color = "#EF4444", Type = TransactionType.Expense },
                new Category { Id = Guid.NewGuid().ToString(), Name = "Salary", Icon = "payments", Color = "#10B981", Type = TransactionType.Income },
                new Category { Id = Guid.NewGuid().ToString(), Name = "Rent", Icon = "home", Color = "#3B82F6", Type = TransactionType.Expense },
                new Category { Id = Guid.NewGuid().ToString(), Name = "Shopping", Icon = "shopping_cart", Color = "#F59E0B", Type = TransactionType.Expense },
                new Category { Id = Guid.NewGuid().ToString(), Name = "Transport", Icon = "directions_car", Color = "#8B5CF6", Type = TransactionType.Expense },
                new Category { Id = Guid.NewGuid().ToString(), Name = "Entertainment", Icon = "movie", Color = "#EC4899", Type = TransactionType.Expense },
                new Category { Id = Guid.NewGuid().ToString(), Name = "Health", Icon = "medical_services", Color = "#14B8A6", Type = TransactionType.Expense },
                new Category { Id = Guid.NewGuid().ToString(), Name = "Utilities", Icon = "bolt", Color = "#06B6D4", Type = TransactionType.Expense }
            };
            await Categories.InsertManyAsync(categories);
        }
    }
}
