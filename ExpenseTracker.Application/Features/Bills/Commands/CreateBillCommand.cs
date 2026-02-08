using MediatR;

namespace ExpenseTracker.Application.Features.Bills.Commands;

public class CreateBillCommand : IRequest<string>
{
    public string Name { get; set; } = string.Empty;
    public decimal Amount { get; set; }
    public string CategoryId { get; set; } = string.Empty;
    public DateTime DueDate { get; set; }
    public string Frequency { get; set; } = "Monthly";
    public bool IsActive { get; set; } = true;
    public string? Notes { get; set; }
}
