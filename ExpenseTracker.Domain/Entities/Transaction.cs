using ExpenseTracker.Domain.Enums;

namespace ExpenseTracker.Domain.Entities;

public class Transaction
{
    public string Id { get; set; } = string.Empty;
    public decimal Amount { get; set; }
    public string CategoryId { get; set; } = string.Empty;
    public string Description { get; set; } = string.Empty;
    public DateTime Date { get; set; }
    public TransactionType Type { get; set; }
    public string Merchant { get; set; } = string.Empty;
    public PaymentMethod PaymentMethod { get; set; }
    public string? ReceiptUrl { get; set; }
    public bool IsRecurring { get; set; }
    public string UserId { get; set; } = string.Empty;
}
