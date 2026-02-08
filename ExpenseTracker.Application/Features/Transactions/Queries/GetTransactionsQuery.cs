using ExpenseTracker.Application.DTOs;
using ExpenseTracker.Domain.Entities;
using MediatR;

namespace ExpenseTracker.Application.Features.Transactions.Queries;

public class GetTransactionsQuery : IRequest<PagedResult<TransactionDto>>
{
    public TransactionFilterParams FilterParams { get; set; } = new();
}

public class TransactionDto
{
    public string Id { get; set; } = string.Empty;
    public decimal Amount { get; set; }
    public string CategoryName { get; set; } = string.Empty;
    public string CategoryIcon { get; set; } = string.Empty;
    public string CategoryColor { get; set; } = string.Empty;
    public string Description { get; set; } = string.Empty;
    public DateTime Date { get; set; }
    public string Type { get; set; } = string.Empty;
    public string Merchant { get; set; } = string.Empty;
    public string PaymentMethod { get; set; } = string.Empty;
    public bool IsRecurring { get; set; }
}
