using ExpenseTracker.Application.Features.Budgets.Commands;
using ExpenseTracker.Application.Interfaces;
using MediatR;

namespace ExpenseTracker.Application.Features.Budgets.Handlers;

public class UpdateBudgetCommandHandler : IRequestHandler<UpdateBudgetCommand, Unit>
{
    private readonly IBudgetRepository _repository;

    public UpdateBudgetCommandHandler(IBudgetRepository repository)
    {
        _repository = repository;
    }

    public async Task<Unit> Handle(UpdateBudgetCommand request, CancellationToken cancellationToken)
    {
        var budget = await _repository.GetAsync(request.Id);
        if (budget == null)
        {
            // In a real app, throw NotFoundException
            return Unit.Value;
        }

        budget.Amount = request.Amount;
        await _repository.UpdateAsync(budget);

        return Unit.Value;
    }
}
