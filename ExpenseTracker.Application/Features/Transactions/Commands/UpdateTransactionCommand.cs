using ExpenseTracker.Application.DTOs;
using MediatR;

namespace ExpenseTracker.Application.Features.Transactions.Commands;

public class UpdateTransactionCommand : IRequest<Unit>
{
    public string Id { get; set; } = string.Empty;
    public CreateTransactionDto Dto { get; set; } = new();
}
