using ExpenseTracker.Application.Features.Bills.Commands;
using ExpenseTracker.Application.Interfaces;
using ExpenseTracker.Domain.Entities;
using MediatR;

namespace ExpenseTracker.Application.Features.Bills.Handlers;

public class CreateBillCommandHandler : IRequestHandler<CreateBillCommand, string>
{
    private readonly IBillRepository _billRepository;
    private readonly ICurrentUserService _currentUserService;

    public CreateBillCommandHandler(
        IBillRepository billRepository,
        ICurrentUserService currentUserService)
    {
        _billRepository = billRepository;
        _currentUserService = currentUserService;
    }

    public async Task<string> Handle(CreateBillCommand request, CancellationToken cancellationToken)
    {
        var userId = _currentUserService.UserId ?? throw new UnauthorizedAccessException("User not authenticated");

        var bill = new Bill
        {
            Id = Guid.NewGuid().ToString(),
            UserId = userId,
            Name = request.Name,
            Amount = request.Amount,
            CategoryId = request.CategoryId,
            DueDate = request.DueDate,
            Frequency = request.Frequency,
            IsActive = request.IsActive,
            IsPaid = false,
            Notes = request.Notes,
            CreatedAt = DateTime.UtcNow
        };

        await _billRepository.AddAsync(bill);
        return bill.Id;
    }
}
