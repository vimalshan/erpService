using FluentValidation;

namespace BankService.Application.Commands.BankMasters;

public class CreateBankMasterValidator : AbstractValidator<CreateBankMasterCommand>
{
    public CreateBankMasterValidator()
    {
        RuleFor(x => x.BankTrustCode).NotEmpty().MaximumLength(3);
        RuleFor(x => x.BankCode).NotEmpty().MaximumLength(6);
        RuleFor(x => x.BankName).NotEmpty().MaximumLength(65);
        RuleFor(x => x.MicrCode).NotEmpty().MaximumLength(9);
        RuleFor(x => x.BranchName).NotEmpty().MaximumLength(65);
        RuleFor(x => x.BranchAddressLine1).NotEmpty().MaximumLength(200);
        RuleFor(x => x.BranchEffDate).NotEmpty();
    }
}
