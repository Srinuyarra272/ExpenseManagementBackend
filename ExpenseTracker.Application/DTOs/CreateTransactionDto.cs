using ExpenseTracker.Domain.Enums;
using Microsoft.AspNetCore.Http;

namespace ExpenseTracker.Application.DTOs;

public class CreateTransactionDto
{
    public decimal Amount { get; set; }
    public string CategoryId { get; set; } = string.Empty;
    public string Description { get; set; } = string.Empty;
    public DateTime Date { get; set; }
    public TransactionType Type { get; set; } // 0 = Expense, 1 = Income
    public string? Merchant { get; set; }
    public PaymentMethod PaymentMethod { get; set; }
    public IFormFile? ReceiptImage { get; set; }
    public bool IsRecurring { get; set; }
}
