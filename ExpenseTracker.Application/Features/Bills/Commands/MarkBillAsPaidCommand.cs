using MediatR;

namespace ExpenseTracker.Application.Features.Bills.Commands;

public class MarkBillAsPaidCommand : IRequest<Unit>
{
    public string Id { get; set; } = string.Empty;
    public bool IsPaid { get; set; }
}
