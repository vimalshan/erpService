using FluentValidation;

namespace AccountingService.Application.Features.MainAccounts.Commands.CreateMainAccount;

public class CreateMainAccountCommandValidator : AbstractValidator<CreateMainAccountCommand>
{
    public CreateMainAccountCommandValidator()
    {
        RuleFor(x => x.MainAccountCode).NotEmpty().MaximumLength(10);
        RuleFor(x => x.MainAccountName).NotEmpty().MaximumLength(200);
        RuleFor(x => x.MainAccountShrtName).MaximumLength(30).When(x => x.MainAccountShrtName != null);
    }
}
