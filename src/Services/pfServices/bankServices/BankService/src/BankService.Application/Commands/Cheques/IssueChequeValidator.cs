using FluentValidation;

namespace BankService.Application.Commands.Cheques;

public class IssueChequeValidator : AbstractValidator<IssueChequeCommand>
{
    public IssueChequeValidator()
    {
        RuleFor(x => x.ChequeId).GreaterThan(0);
        RuleFor(x => x.ChequeNo).GreaterThan(0);
        RuleFor(x => x.Amount).GreaterThan(0);
        RuleFor(x => x.ChequeDate).NotEmpty();
        RuleFor(x => x.Payee).NotEmpty().MaximumLength(100);
    }
}
