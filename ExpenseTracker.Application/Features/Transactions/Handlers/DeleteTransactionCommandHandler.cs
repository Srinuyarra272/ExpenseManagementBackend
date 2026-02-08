using ExpenseTracker.Application.Features.Transactions.Commands;
using ExpenseTracker.Application.Interfaces;
using MediatR;

namespace ExpenseTracker.Application.Features.Transactions.Handlers;

public class DeleteTransactionCommandHandler : IRequestHandler<DeleteTransactionCommand>
{
    private readonly ITransactionRepository _repository;
    private readonly ICurrentUserService _currentUserService;

    public DeleteTransactionCommandHandler(ITransactionRepository repository, ICurrentUserService currentUserService)
    {
        _repository = repository;
        _currentUserService = currentUserService;
    }

    public async Task Handle(DeleteTransactionCommand request, CancellationToken cancellationToken)
    {
        var userId = _currentUserService.UserId ?? throw new UnauthorizedAccessException("User not authenticated");
        await _repository.DeleteAsync(request.Id, userId);
    }
}
