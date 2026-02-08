namespace ExpenseTracker.Application.DTOs;

public class BillDto
{
    public string Id { get; set; } = string.Empty;
    public string Name { get; set; } = string.Empty;
    public decimal Amount { get; set; }
    public string CategoryId { get; set; } = string.Empty;
    public string CategoryName { get; set; } = string.Empty;
    public string CategoryIcon { get; set; } = string.Empty;
    public string CategoryColor { get; set; } = string.Empty;
    public DateTime DueDate { get; set; }
    public string Frequency { get; set; } = string.Empty;
    public bool IsActive { get; set; }
    public bool IsPaid { get; set; }
    public string? Notes { get; set; }
}

public class CreateBillDto
{
    public string Name { get; set; } = string.Empty;
    public decimal Amount { get; set; }
    public string CategoryId { get; set; } = string.Empty;
    public DateTime DueDate { get; set; }
    public string Frequency { get; set; } = "Monthly";
    public bool IsActive { get; set; } = true;
    public string? Notes { get; set; }
}
