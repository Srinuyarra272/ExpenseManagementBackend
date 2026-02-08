namespace ExpenseTracker.Domain.Entities;

public class Budget
{
    public string Id { get; set; } = string.Empty;
    public string CategoryId { get; set; } = string.Empty;
    public decimal Amount { get; set; }
    public int Month { get; set; }
    public int Year { get; set; }
    public string UserId { get; set; } = string.Empty;
}
