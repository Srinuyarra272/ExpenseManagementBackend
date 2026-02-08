using ExpenseTracker.Domain.Enums;

namespace ExpenseTracker.Domain.Entities;

public class Category
{
    public string Id { get; set; } = string.Empty;
    public string Name { get; set; } = string.Empty;
    public string Icon { get; set; } = string.Empty; // Material Icon name
    public string Color { get; set; } = string.Empty; // Hex code
    public TransactionType Type { get; set; }
    public string UserId { get; set; } = string.Empty;
}
