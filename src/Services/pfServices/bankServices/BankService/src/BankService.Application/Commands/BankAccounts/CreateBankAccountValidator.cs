using FluentValidation;

namespace BankService.Application.Commands.BankAccounts;

public class CreateBankAccountValidator : AbstractValidator<CreateBankAccountCommand>
{
    public CreateBankAccountValidator()
    {
        RuleFor(x => x.AccountNumber).NotEmpty().MaximumLength(30);
        RuleFor(x => x.AccountTitle).NotEmpty().MaximumLength(100);
        RuleFor(x => x.BankCode).NotEmpty().MaximumLength(6);
        RuleFor(x => x.TrustCode).NotEmpty().MaximumLength(3);
        RuleFor(x => x.AccountType).NotEmpty().MaximumLength(20);
        RuleFor(x => x.OpeningDate).NotEmpty();
    }
}
