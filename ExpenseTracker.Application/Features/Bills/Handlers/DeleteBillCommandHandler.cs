using ExpenseTracker.Application.Features.Bills.Commands;
using ExpenseTracker.Application.Interfaces;
using MediatR;

namespace ExpenseTracker.Application.Features.Bills.Handlers;

public class DeleteBillCommandHandler : IRequestHandler<DeleteBillCommand, Unit>
{
    private readonly IBillRepository _billRepository;

    public DeleteBillCommandHandler(IBillRepository billRepository)
    {
        _billRepository = billRepository;
    }

    public async Task<Unit> Handle(DeleteBillCommand request, CancellationToken cancellationToken)
    {
        await _billRepository.DeleteAsync(request.Id);
        return Unit.Value;
    }
}
