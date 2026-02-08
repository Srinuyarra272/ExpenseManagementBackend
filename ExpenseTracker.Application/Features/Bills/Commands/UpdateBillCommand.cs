using MediatR;

namespace ExpenseTracker.Application.Features.Bills.Commands;

public class UpdateBillCommand : IRequest<Unit>
{
    public string Id { get; set; } = string.Empty;
    public string Name { get; set; } = string.Empty;
    public decimal Amount { get; set; }
    public string CategoryId { get; set; } = string.Empty;
    public DateTime DueDate { get; set; }
    public string Frequency { get; set; } = "Monthly";
    public bool IsActive { get; set; }
    public string? Notes { get; set; }
}
