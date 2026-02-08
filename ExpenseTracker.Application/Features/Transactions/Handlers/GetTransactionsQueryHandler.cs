using ExpenseTracker.Application.DTOs;
using ExpenseTracker.Application.Features.Transactions.Queries;
using ExpenseTracker.Application.Interfaces;
using MediatR;

namespace ExpenseTracker.Application.Features.Transactions.Handlers;

public class GetTransactionsQueryHandler : IRequestHandler<GetTransactionsQuery, PagedResult<TransactionDto>>
{
    private readonly ITransactionRepository _transactionRepository;
    private readonly ICategoryRepository _categoryRepository;
    private readonly ICurrentUserService _currentUserService;

    public GetTransactionsQueryHandler(
        ITransactionRepository transactionRepository,
        ICategoryRepository categoryRepository,
        ICurrentUserService currentUserService)
    {
        _transactionRepository = transactionRepository;
        _categoryRepository = categoryRepository;
        _currentUserService = currentUserService;
    }

    public async Task<PagedResult<TransactionDto>> Handle(GetTransactionsQuery request, CancellationToken cancellationToken)
    {
        var userId = _currentUserService.UserId ?? throw new UnauthorizedAccessException("User not authenticated");
        var pagedResult = await _transactionRepository.GetFilteredAsync(userId, request.FilterParams);

        var categoryIds = pagedResult.Items.Select(t => t.CategoryId).Distinct();
        var categories = await _categoryRepository.GetByIdsAsync(categoryIds);

        var dtos = pagedResult.Items.Select(t =>
        {
            var category = categories.FirstOrDefault(c => c.Id == t.CategoryId);
            return new TransactionDto
            {
                Id = t.Id,
                Amount = t.Amount,
                CategoryName = category?.Name ?? "Unknown",
                CategoryIcon = category?.Icon ?? "help_outline",
                CategoryColor = category?.Color ?? "#000000",
                Description = t.Description,
                Date = t.Date,
                Type = t.Type.ToString(),
                Merchant = t.Merchant,
                PaymentMethod = t.PaymentMethod.ToString(),
                IsRecurring = t.IsRecurring
            };
        }).ToList();

        return new PagedResult<TransactionDto>
        {
            Items = dtos,
            TotalCount = pagedResult.TotalCount,
            PageNumber = pagedResult.PageNumber,
            PageSize = pagedResult.PageSize
        };
    }
}
