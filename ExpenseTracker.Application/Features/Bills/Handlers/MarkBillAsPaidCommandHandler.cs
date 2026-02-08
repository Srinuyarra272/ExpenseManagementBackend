using ExpenseTracker.Application.Features.Bills.Commands;
using ExpenseTracker.Application.Interfaces;
using ExpenseTracker.Domain.Entities;
using ExpenseTracker.Domain.Enums;
using MediatR;

namespace ExpenseTracker.Application.Features.Bills.Handlers;

public class MarkBillAsPaidCommandHandler : IRequestHandler<MarkBillAsPaidCommand, Unit>
{
    private readonly IBillRepository _billRepository;
    private readonly ITransactionRepository _transactionRepository;

    public MarkBillAsPaidCommandHandler(IBillRepository billRepository, ITransactionRepository transactionRepository)
    {
        _billRepository = billRepository;
        _transactionRepository = transactionRepository;
    }

    public async Task<Unit> Handle(MarkBillAsPaidCommand request, CancellationToken cancellationToken)
    {
        var bill = await _billRepository.GetAsync(request.Id);
        if (bill == null)
            throw new KeyNotFoundException($"Bill with ID {request.Id} not found");

        // Case 1: Marking as PAID
        if (request.IsPaid && !bill.IsPaid)
        {
            var transaction = new Transaction
            {
                Id = Guid.NewGuid().ToString(),
                UserId = bill.UserId,
                Amount = bill.Amount,
                Date = DateTime.UtcNow,
                Description = $"Bill Payment: {bill.Name}",
                CategoryId = bill.CategoryId,
                Type = TransactionType.Expense,
                Merchant = bill.Name,
                PaymentMethod = PaymentMethod.BankTransfer,
                IsRecurring = bill.Frequency != "OneTime"
            };
            
            await _transactionRepository.AddAsync(transaction);
            bill.PaymentTransactionId = transaction.Id;
        }
        // Case 2: Unmarking (Reverting to UNPAID)
        else if (!request.IsPaid && bill.IsPaid)
        {
            if (!string.IsNullOrEmpty(bill.PaymentTransactionId))
            {
                await _transactionRepository.DeleteAsync(bill.PaymentTransactionId, bill.UserId);
                bill.PaymentTransactionId = null;
            }
        }

        bill.IsPaid = request.IsPaid;
        await _billRepository.UpdateAsync(bill);
        return Unit.Value;
    }
}
