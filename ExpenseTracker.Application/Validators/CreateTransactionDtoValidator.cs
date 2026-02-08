using ExpenseTracker.Application.DTOs;
using FluentValidation;

namespace ExpenseTracker.Application.Validators;

public class CreateTransactionDtoValidator : AbstractValidator<CreateTransactionDto>
{
    public CreateTransactionDtoValidator()
    {
        RuleFor(x => x.Amount).GreaterThan(0).WithMessage("Amount must be greater than 0");
        RuleFor(x => x.CategoryId).NotEmpty().WithMessage("Category is required");
        RuleFor(x => x.Date).NotEmpty().WithMessage("Date is required");
        RuleFor(x => x.Description).NotEmpty().WithMessage("Description is required");
    }
}
