using ExpenseTracker.Application.DTOs;
using MediatR;

namespace ExpenseTracker.Application.Features.Budgets.Queries;

public record GetBudgetsQuery(int Month, int Year) : IRequest<List<BudgetDto>>;
