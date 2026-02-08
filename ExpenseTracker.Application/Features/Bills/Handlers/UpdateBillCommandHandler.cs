using ExpenseTracker.Application.Features.Bills.Commands;
using ExpenseTracker.Application.Interfaces;
using ExpenseTracker.Domain.Entities;
using MediatR;

namespace ExpenseTracker.Application.Features.Bills.Handlers;

public class UpdateBillCommandHandler : IRequestHandler<UpdateBillCommand, Unit>
{
    private readonly IBillRepository _billRepository;

    public UpdateBillCommandHandler(IBillRepository billRepository)
    {
        _billRepository = billRepository;
    }

    public async Task<Unit> Handle(UpdateBillCommand request, CancellationToken cancellationToken)
    {
        var bill = await _billRepository.GetAsync(request.Id);
        if (bill == null)
            throw new KeyNotFoundException($"Bill with ID {request.Id} not found");

        bill.Name = request.Name;
        bill.Amount = request.Amount;
        bill.CategoryId = request.CategoryId;
        bill.DueDate = request.DueDate;
        bill.Frequency = request.Frequency;
        bill.IsActive = request.IsActive;
        bill.Notes = request.Notes;

        await _billRepository.UpdateAsync(bill);
        return Unit.Value;
    }
}
