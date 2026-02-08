using ExpenseTracker.Application.DTOs;
using ExpenseTracker.Application.Features.Budgets.Queries;
using ExpenseTracker.Application.Interfaces;
using ExpenseTracker.Domain.Enums;
using MediatR;

namespace ExpenseTracker.Application.Features.Budgets.Handlers;

public class GetBudgetsQueryHandler : IRequestHandler<GetBudgetsQuery, List<BudgetDto>>
{
    private readonly IBudgetRepository _budgetRepository;
    private readonly ITransactionRepository _transactionRepository;
    private readonly ICategoryRepository _categoryRepository;
    private readonly ICurrentUserService _currentUserService;

    public GetBudgetsQueryHandler(
        IBudgetRepository budgetRepository,
        ITransactionRepository transactionRepository,
        ICategoryRepository categoryRepository,
        ICurrentUserService currentUserService)
    {
        _budgetRepository = budgetRepository;
        _transactionRepository = transactionRepository;
        _categoryRepository = categoryRepository;
        _currentUserService = currentUserService;
    }

    public async Task<List<BudgetDto>> Handle(GetBudgetsQuery request, CancellationToken cancellationToken)
    {
        var userId = _currentUserService.UserId ?? "test-user";
        var budgets = await _budgetRepository.GetByMonthAsync(userId, request.Month, request.Year);
        var categories = await _categoryRepository.GetAllAsync();
        
        // We need to calculate spent amount per category for the given month/year
        // This is expensive if we query transactions one by one.
        // Ideally, TransactionRepository should have a method to get aggregation by category.
        // For now, let's fetch all transactions for the month (optimized approach would be aggregation query)
        
        var transactions = await _transactionRepository.GetByMonthAsync(userId, request.Month, request.Year);

        var dtos = new List<BudgetDto>();

        foreach (var budget in budgets)
        {
            var category = categories.FirstOrDefault(c => c.Id == budget.CategoryId);
            var spent = transactions
                .Where(t => t.CategoryId == budget.CategoryId && t.Type == TransactionType.Expense)
                .Sum(t => t.Amount);

            dtos.Add(new BudgetDto
            {
                Id = budget.Id,
                CategoryId = budget.CategoryId,
                CategoryName = category?.Name ?? "Unknown",
                CategoryColor = category?.Color ?? "#808080",
                CategoryIcon = category?.Icon ?? "help",
                Amount = budget.Amount,
                Spent = spent,
                Month = budget.Month,
                Year = budget.Year
            });
        }

        return dtos;
    }
}
