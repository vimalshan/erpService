using FluentValidation;
using SwipeTransactionService.Application.Features.SwipeTransactions.Commands;

namespace SwipeTransactionService.Application.Validators;

public sealed class RecordSwipeUploadValidator : AbstractValidator<RecordSwipeUploadCommand>
{
    public RecordSwipeUploadValidator()
    {
        RuleFor(x => x.EmployeeNumber).NotEmpty().MaximumLength(20);
        RuleFor(x => x.CompanyCode).GreaterThan(0);
        RuleFor(x => x.ItemCode).GreaterThan(0);
        RuleFor(x => x.ItemQuantity).GreaterThan(0);
        RuleFor(x => x.BatchNumber).GreaterThan(0);
        RuleFor(x => x.SerialNumber).GreaterThan(0);
        RuleFor(x => x.GateNumber).NotEmpty().MaximumLength(3);
        RuleFor(x => x.SwipeTime).LessThanOrEqualTo(DateTime.UtcNow.AddMinutes(5))
            .WithMessage("Swipe time cannot be in the future.");
    }
}
