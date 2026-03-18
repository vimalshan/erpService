using FluentValidation;

namespace AccountingService.Application.Features.AccountDetails.Commands.CreateAccountDetail;

public class CreateAccountDetailCommandValidator : AbstractValidator<CreateAccountDetailCommand>
{
    public CreateAccountDetailCommandValidator()
    {
        RuleFor(x => x.AcTrustCode).NotEmpty().Length(3);
        RuleFor(x => x.AcTranCode).NotEmpty().MaximumLength(3);
        RuleFor(x => x.AcMainCode).NotEmpty().MaximumLength(6);
        RuleFor(x => x.AcSubCode).NotEmpty().MaximumLength(6);
        RuleFor(x => x.AcDcType).Must(t => t == "D" || t == "C").WithMessage("DC Type must be 'D' or 'C'.");
        RuleFor(x => x.AcTranAmt).GreaterThan(0);
        RuleFor(x => x.AcDocDat).NotEmpty().LessThanOrEqualTo(DateTime.UtcNow.AddDays(1));
        RuleFor(x => x.AcRemarks).MaximumLength(2000).When(x => x.AcRemarks != null);
    }
}
