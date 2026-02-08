using MediatR;

namespace ExpenseTracker.Application.Features.Budgets.Commands;

public record UpdateBudgetCommand(string Id, decimal Amount) : IRequest<Unit>;
