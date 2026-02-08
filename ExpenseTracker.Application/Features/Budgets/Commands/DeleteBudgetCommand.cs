using MediatR;

namespace ExpenseTracker.Application.Features.Budgets.Commands;

public record DeleteBudgetCommand(string Id) : IRequest<Unit>;
