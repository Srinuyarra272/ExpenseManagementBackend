using ExpenseTracker.Application.Features.Transactions.Commands;
using ExpenseTracker.Application.Interfaces;
using MediatR;

namespace ExpenseTracker.Application.Features.Transactions.Handlers;

public class UpdateTransactionCommandHandler : IRequestHandler<UpdateTransactionCommand, Unit>
{
    private readonly ITransactionRepository _repository;
    private readonly IFileStorageService _fileStorage;
    private readonly ICurrentUserService _currentUserService;

    public UpdateTransactionCommandHandler(ITransactionRepository repository, IFileStorageService fileStorage, ICurrentUserService currentUserService)
    {
        _repository = repository;
        _fileStorage = fileStorage;
        _currentUserService = currentUserService;
    }

    public async Task<Unit> Handle(UpdateTransactionCommand request, CancellationToken cancellationToken)
    {
        var transaction = await _repository.GetByIdAsync(request.Id, _currentUserService.UserId ?? string.Empty);
        
        if (transaction == null)
        {
            throw new Exception("Transaction not found");
        }

        if (request.Dto.ReceiptImage != null)
        {
            transaction.ReceiptUrl = await _fileStorage.SaveFileAsync(request.Dto.ReceiptImage, "receipts");
        }

        transaction.Amount = request.Dto.Amount;
        transaction.CategoryId = request.Dto.CategoryId;
        transaction.Description = request.Dto.Description;
        transaction.Date = request.Dto.Date;
        transaction.Type = request.Dto.Type;
        transaction.Merchant = request.Dto.Merchant;
        transaction.PaymentMethod = request.Dto.PaymentMethod;
        transaction.IsRecurring = request.Dto.IsRecurring;

        await _repository.UpdateAsync(transaction);

        return Unit.Value;
    }
}
