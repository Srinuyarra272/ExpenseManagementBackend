using ExpenseTracker.Application.DTOs;
using MediatR;

namespace ExpenseTracker.Application.Features.Budgets.Commands;

public record CreateBudgetCommand(CreateBudgetDto Budget) : IRequest<string>;
