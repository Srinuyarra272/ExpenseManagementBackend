namespace ExpenseTracker.Domain.Entities;

public class Bill
{
    public string Id { get; set; } = string.Empty;
    public string UserId { get; set; } = string.Empty;
    public string Name { get; set; } = string.Empty;
    public decimal Amount { get; set; }
    public string CategoryId { get; set; } = string.Empty;
    public DateTime DueDate { get; set; }
    public string Frequency { get; set; } = "Monthly"; // Monthly, Quarterly, Yearly
    public bool IsActive { get; set; } = true;
    public bool IsPaid { get; set; } = false;
    public string? PaymentTransactionId { get; set; }
    public string? Notes { get; set; }
    public DateTime CreatedAt { get; set; } = DateTime.UtcNow;
}
