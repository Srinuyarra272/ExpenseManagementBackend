namespace ExpenseTracker.Application.DTOs;

public class BudgetDto
{
    public string Id { get; set; } = string.Empty;
    public string CategoryId { get; set; } = string.Empty;
    public string CategoryName { get; set; } = string.Empty;
    public string CategoryColor { get; set; } = string.Empty;
    public string CategoryIcon { get; set; } = string.Empty;
    public decimal Amount { get; set; }
    public decimal Spent { get; set; }
    public decimal Remaining => Amount - Spent;
    public double Percentage => Amount > 0 ? (double)(Spent / Amount) * 100 : 0;
    public int Month { get; set; }
    public int Year { get; set; }
}

public class CreateBudgetDto
{
    public string CategoryId { get; set; } = string.Empty;
    public decimal Amount { get; set; }
    public int Month { get; set; }
    public int Year { get; set; }
}
