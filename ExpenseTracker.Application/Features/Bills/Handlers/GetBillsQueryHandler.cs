using ExpenseTracker.Application.DTOs;
using ExpenseTracker.Application.Features.Bills.Queries;
using ExpenseTracker.Application.Interfaces;
using MediatR;

namespace ExpenseTracker.Application.Features.Bills.Handlers;

public class GetBillsQueryHandler : IRequestHandler<GetBillsQuery, List<BillDto>>
{
    private readonly IBillRepository _billRepository;
    private readonly ICategoryRepository _categoryRepository;
    private readonly ICurrentUserService _currentUserService;

    public GetBillsQueryHandler(
        IBillRepository billRepository,
        ICategoryRepository categoryRepository,
        ICurrentUserService currentUserService)
    {
        _billRepository = billRepository;
        _categoryRepository = categoryRepository;
        _currentUserService = currentUserService;
    }

    public async Task<List<BillDto>> Handle(GetBillsQuery request, CancellationToken cancellationToken)
    {
        var userId = _currentUserService.UserId ?? throw new UnauthorizedAccessException("User not authenticated");
        
        var bills = await _billRepository.GetAllAsync(userId);
        var categoryIds = bills.Select(b => b.CategoryId).Distinct();
        var categories = await _categoryRepository.GetByIdsAsync(categoryIds);

        return bills.Select(b =>
        {
            var category = categories.FirstOrDefault(c => c.Id == b.CategoryId);
            return new BillDto
            {
                Id = b.Id,
                Name = b.Name,
                Amount = b.Amount,
                CategoryId = b.CategoryId,
                CategoryName = category?.Name ?? "Unknown",
                CategoryIcon = category?.Icon ?? "help_outline",
                CategoryColor = category?.Color ?? "#gray",
                DueDate = b.DueDate,
                Frequency = b.Frequency,
                IsActive = b.IsActive,
                IsPaid = b.IsPaid,
                Notes = b.Notes
            };
        }).OrderBy(b => b.DueDate).ToList();
    }
}
