using ExpenseTracker.Application.Features.Transactions.Commands;
using ExpenseTracker.Application.Interfaces;
using ExpenseTracker.Domain.Entities;
using MediatR;

namespace ExpenseTracker.Application.Features.Transactions.Handlers;

public class CreateTransactionCommandHandler : IRequestHandler<CreateTransactionCommand, string>
{
    private readonly ITransactionRepository _repository;
    private readonly IFileStorageService _fileStorage;
    private readonly ICurrentUserService _currentUserService;

    public CreateTransactionCommandHandler(ITransactionRepository repository, IFileStorageService fileStorage, ICurrentUserService currentUserService)
    {
        _repository = repository;
        _fileStorage = fileStorage;
        _currentUserService = currentUserService;
    }

    public async Task<string> Handle(CreateTransactionCommand request, CancellationToken cancellationToken)
    {
        string? receiptUrl = null;

        if (request.Dto.ReceiptImage != null)
        {
            receiptUrl = await _fileStorage.SaveFileAsync(request.Dto.ReceiptImage, "receipts");
        }

        var transaction = new Transaction
        {
            Id = Guid.NewGuid().ToString(),
            Amount = request.Dto.Amount,
            CategoryId = request.Dto.CategoryId,
            Description = request.Dto.Description,
            Date = request.Dto.Date,
            Type = request.Dto.Type,
            Merchant = request.Dto.Merchant,
            PaymentMethod = request.Dto.PaymentMethod,
            IsRecurring = request.Dto.IsRecurring,
            ReceiptUrl = receiptUrl,
            UserId = _currentUserService.UserId ?? request.UserId // Use authenticated user, fallback to request for testing if needed
        };

        await _repository.AddAsync(transaction);

        return transaction.Id;
    }
}
