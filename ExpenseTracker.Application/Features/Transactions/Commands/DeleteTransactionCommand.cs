using MediatR;

namespace ExpenseTracker.Application.Features.Transactions.Commands;

public class DeleteTransactionCommand : IRequest
{
    public string Id { get; set; } = string.Empty;
}
