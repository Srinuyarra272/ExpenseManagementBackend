using ExpenseTracker.Application.Features.Budgets.Commands;
using ExpenseTracker.Application.Interfaces;
using ExpenseTracker.Domain.Entities;
using MediatR;

namespace ExpenseTracker.Application.Features.Budgets.Handlers;

public class CreateBudgetCommandHandler : IRequestHandler<CreateBudgetCommand, string>
{
    private readonly IBudgetRepository _repository;
    private readonly ICurrentUserService _currentUserService;

    public CreateBudgetCommandHandler(IBudgetRepository repository, ICurrentUserService currentUserService)
    {
        _repository = repository;
        _currentUserService = currentUserService;
    }

    public async Task<string> Handle(CreateBudgetCommand request, CancellationToken cancellationToken)
    {
        var budget = new Budget
        {
            Id = Guid.NewGuid().ToString(),
            CategoryId = request.Budget.CategoryId,
            Amount = request.Budget.Amount,
            Month = request.Budget.Month,
            Year = request.Budget.Year,
            UserId = _currentUserService.UserId ?? "test-user"
        };

        await _repository.AddAsync(budget);
        return budget.Id;
    }
}
