using FluentValidation;

namespace AccountingService.Application.Features.Transactions.Commands.CreateTransaction;

public class CreateTransactionCommandValidator : AbstractValidator<CreateTransactionCommand>
{
    public CreateTransactionCommandValidator()
    {
        RuleFor(x => x.TrustCode).NotEmpty().Length(3);
        RuleFor(x => x.TransactionCode).NotEmpty().MaximumLength(3);
        RuleFor(x => x.TransactionDate).NotEmpty();
        RuleFor(x => x.Amount).GreaterThan(0);
        RuleFor(x => x.TypeCode).NotEmpty().MaximumLength(1);
        RuleFor(x => x.ModifiedBy).NotEmpty().MaximumLength(50);
        RuleFor(x => x.JvVoucherType).NotEmpty().MaximumLength(3);
        RuleFor(x => x.JvNo).NotEmpty().MaximumLength(255);
    }
}
