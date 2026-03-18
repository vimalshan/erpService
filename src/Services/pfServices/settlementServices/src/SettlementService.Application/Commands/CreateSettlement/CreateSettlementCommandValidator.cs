using FluentValidation;

namespace SettlementService.Application.Commands.CreateSettlement;

public class CreateSettlementCommandValidator : AbstractValidator<CreateSettlementCommand>
{
    public CreateSettlementCommandValidator()
    {
        RuleFor(x => x.SettlementNumber).GreaterThan(0);
        RuleFor(x => x.MemberNo).GreaterThan(0);
        RuleFor(x => x.SettlementType).NotEmpty().MaximumLength(1);
        RuleFor(x => x.SettlementAmount).GreaterThan(0);
        RuleFor(x => x.SettlementDate).NotEmpty();
        RuleFor(x => x.CreatedBy).GreaterThan(0);
        RuleFor(x => x.TrustCode).MaximumLength(3).When(x => x.TrustCode != null);
    }
}
