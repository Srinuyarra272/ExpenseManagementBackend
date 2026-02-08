using ExpenseTracker.Application.DTOs;
using ExpenseTracker.Application.Features.Dashboard.Queries;
using ExpenseTracker.Application.Interfaces;
using ExpenseTracker.Domain.Enums;
using MediatR;

namespace ExpenseTracker.Application.Features.Dashboard.Handlers;

public class GetDashboardMetricsQueryHandler : IRequestHandler<GetDashboardMetricsQuery, DashboardMetricsDto>
{
    private readonly ITransactionRepository _transactionRepository;
    private readonly IBudgetRepository _budgetRepository;
    private readonly ICategoryRepository _categoryRepository;
    private readonly IBillRepository _billRepository;
    private readonly ICurrentUserService _currentUserService;

    public GetDashboardMetricsQueryHandler(
        ITransactionRepository transactionRepository,
        IBudgetRepository budgetRepository,
        ICategoryRepository categoryRepository,
        IBillRepository billRepository,
        ICurrentUserService currentUserService)
    {
        _transactionRepository = transactionRepository;
        _budgetRepository = budgetRepository;
        _categoryRepository = categoryRepository;
        _billRepository = billRepository;
        _currentUserService = currentUserService;
    }

    public async Task<DashboardMetricsDto> Handle(GetDashboardMetricsQuery request, CancellationToken cancellationToken)
    {
        var utcNow = DateTime.UtcNow;
        var year = request.Year ?? utcNow.Year;
        var month = request.Month ?? utcNow.Month;
        
        // Use current day if we're in the current month/year, otherwise cap at days in month
        var daysInMonth = DateTime.DaysInMonth(year, month);
        var day = (year == utcNow.Year && month == utcNow.Month) ? utcNow.Day : daysInMonth;
        
        // This 'now' effectively acts as the cursor for the dashboard view
        var viewDate = new DateTime(year, month, day);

        var startOfMonth = new DateTime(year, month, 1);
        var endOfMonth = startOfMonth.AddMonths(1).AddTicks(-1);
        
        // Calculate week relative to the view date
        var startOfWeek = viewDate.AddDays(-(int)viewDate.DayOfWeek);
        var endOfWeek = startOfWeek.AddDays(7).AddTicks(-1);

        var userId = _currentUserService.UserId ?? throw new UnauthorizedAccessException("User not authenticated");

        // Execute sequentially to ensure stability and avoid concurrency issues
        var totalSpent = await _transactionRepository.GetTotalSpentAsync(userId, startOfMonth, endOfMonth);
        var totalIncome = await _transactionRepository.GetTotalAmountAsync(userId, startOfMonth, endOfMonth, TransactionType.Income);
        var totalSpentThisWeek = await _transactionRepository.GetTotalSpentAsync(userId, startOfWeek, endOfWeek);
        var transactionCount = await _transactionRepository.GetTransactionCountAsync(userId, startOfMonth, endOfMonth);
        var topCategories = await _transactionRepository.GetTopCategoriesAsync(userId, startOfMonth, endOfMonth, 100);
        var recentTransactions = await _transactionRepository.GetRecentTransactionsAsync(userId, startOfMonth, endOfMonth, 5);
        var totalBudget = await _budgetRepository.GetTotalBudgetAsync(userId, month, year);
        
        // Last month data for savings rate comparison
        var lastMonthStart = startOfMonth.AddMonths(-1);
        var lastMonthEnd = startOfMonth.AddTicks(-1);
        var lastMonthSpent = await _transactionRepository.GetTotalSpentAsync(userId, lastMonthStart, lastMonthEnd);
        var lastMonthIncome = await _transactionRepository.GetTotalAmountAsync(userId, lastMonthStart, lastMonthEnd, TransactionType.Income);

        // Enhance Recent Transactions with Category Details
        var catIds = recentTransactions.Select(t => t.CategoryId).Distinct();
        var categories = await _categoryRepository.GetByIdsAsync(catIds);
        
        var recentDtos = recentTransactions.Select(t => 
        {
            var cat = categories.FirstOrDefault(c => c.Id == t.CategoryId);
            return new RecentTransactionDto
            {
                Id = t.Id,
                Description = t.Description,
                Amount = t.Amount,
                Date = t.Date,
                CategoryName = cat?.Name ?? "Uncategorized",
                Type = t.Type.ToString(),
                Icon = cat?.Icon ?? "help_outline"
            };
        }).ToList();

        // Calculate other metrics
        var budgetUtilization = totalBudget > 0 ? (totalSpent / totalBudget) * 100 : 0;
        var avgDaily = viewDate.Day > 0 ? totalSpent / viewDate.Day : totalSpent;
        
        var cashflow = totalIncome - totalSpent;
        
        var currentSavingsRate = totalIncome > 0 ? ((totalIncome - totalSpent) / totalIncome) * 100 : 0;
        var lastMonthSavingsRate = lastMonthIncome > 0 ? ((lastMonthIncome - lastMonthSpent) / lastMonthIncome) * 100 : 0;
        var savingsRateComparison = currentSavingsRate - lastMonthSavingsRate;

        // Fetch upcoming bills (unpaid, active bills for the current month)
        var upcomingBills = await _billRepository.GetUpcomingAsync(userId, startOfMonth, endOfMonth);
        var upcomingBillsCount = upcomingBills.Count();
        var upcomingBillsTotal = upcomingBills.Sum(b => b.Amount);

        // Calculate Daily Trends (Cumulative Spending)
        var dailyStats = await _transactionRepository.GetDailySpendingAsync(userId, startOfMonth, endOfMonth);
        var dailyTrends = new List<DailyTrendDto>();
        decimal cumulative = 0;
        
        for (int i = 1; i <= day; i++)
        {
            var date = new DateTime(year, month, i);
            var stat = dailyStats.FirstOrDefault(s => s.Date.Date == date.Date);
            var dailyAmount = stat?.TotalAmount ?? 0;
            cumulative += dailyAmount;
            
            dailyTrends.Add(new DailyTrendDto
            {
                Date = date,
                Day = i,
                DailyAmount = dailyAmount,
                CumulativeAmount = cumulative
            });
        }

        return new DashboardMetricsDto
        {
            TotalSpentThisMonth = totalSpent,
            TotalSpentThisWeek = totalSpentThisWeek,
            BudgetUtilizationPercentage = Math.Round(budgetUtilization, 1),
            TopCategories = topCategories.Select(c => new CategoryChartData
            {
                Name = c.CategoryName,
                Value = c.TotalAmount,
                Color = c.Color
            }).ToList(),
            TransactionCount = transactionCount,
            AverageDailySpend = Math.Round(avgDaily, 2),
            RecentTransactions = recentDtos,
            UpcomingBillsCount = upcomingBillsCount,
            UpcomingBillsTotal = upcomingBillsTotal,
            SavingsRate = Math.Round(savingsRateComparison, 1),
            Cashflow = cashflow,
            DailyTrends = dailyTrends
        };
    }
}
