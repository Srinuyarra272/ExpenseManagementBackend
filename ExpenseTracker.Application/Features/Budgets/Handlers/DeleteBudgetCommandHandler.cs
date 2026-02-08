using ExpenseTracker.Application.Features.Budgets.Commands;
using ExpenseTracker.Application.Interfaces;
using MediatR;

namespace ExpenseTracker.Application.Features.Budgets.Handlers;

public class DeleteBudgetCommandHandler : IRequestHandler<DeleteBudgetCommand, Unit>
{
    private readonly IBudgetRepository _repository;

    public DeleteBudgetCommandHandler(IBudgetRepository repository)
    {
        _repository = repository;
    }

    public async Task<Unit> Handle(DeleteBudgetCommand request, CancellationToken cancellationToken)
    {
        await _repository.DeleteAsync(request.Id);
        return Unit.Value;
    }
}
