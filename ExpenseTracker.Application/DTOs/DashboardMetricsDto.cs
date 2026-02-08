namespace ExpenseTracker.Application.DTOs;

public class DashboardMetricsDto
{
    public decimal TotalSpentThisMonth { get; set; }
    public decimal TotalSpentThisWeek { get; set; }
    public decimal BudgetUtilizationPercentage { get; set; }
    public List<CategoryChartData> TopCategories { get; set; } = new();
    public int UpcomingBillsCount { get; set; }
    public decimal UpcomingBillsTotal { get; set; }
    public decimal SavingsRate { get; set; } // Comparison vs last month %
    public int TransactionCount { get; set; }
    public decimal AverageDailySpend { get; set; }
    public decimal Cashflow { get; set; } // Income - Expenses
    public List<RecentTransactionDto> RecentTransactions { get; set; } = new();
    public List<DailyTrendDto> DailyTrends { get; set; } = new();
}

public class DailyTrendDto
{
    public DateTime Date { get; set; }
    public int Day { get; set; }
    public decimal DailyAmount { get; set; }
    public decimal CumulativeAmount { get; set; }
}

public class CategoryChartData
{
    public string Name { get; set; } = string.Empty;
    public decimal Value { get; set; }
    public string Color { get; set; } = string.Empty;
}

public class RecentTransactionDto
{
    public string Id { get; set; } = string.Empty;
    public string Description { get; set; } = string.Empty;
    public decimal Amount { get; set; }
    public DateTime Date { get; set; }
    public string CategoryName { get; set; } = string.Empty;
    public string Type { get; set; } = string.Empty;
    public string Icon { get; set; } = string.Empty;
}
