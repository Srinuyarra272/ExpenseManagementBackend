using ExpenseTracker.Application.DTOs;
using MediatR;

namespace ExpenseTracker.Application.Features.Transactions.Commands;

public class CreateTransactionCommand : IRequest<string>
{
    public CreateTransactionDto Dto { get; set; } = new();
    public string UserId { get; set; } = string.Empty;
}
