using MediatR;

namespace ExpenseTracker.Application.Features.Bills.Commands;

public class DeleteBillCommand : IRequest<Unit>
{
    public string Id { get; set; } = string.Empty;
}
