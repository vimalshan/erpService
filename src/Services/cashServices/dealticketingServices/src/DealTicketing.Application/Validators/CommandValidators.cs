using FluentValidation;
using DealTicketing.Application.Features.DealBatches.Commands;
using DealTicketing.Application.Features.DealDetails.Commands;

namespace DealTicketing.Application.Validators;

public class CreateDealBatchCommandValidator : AbstractValidator<CreateDealBatchCommand>
{
    public CreateDealBatchCommandValidator()
    {
        RuleFor(x => x.DealBatchId).GreaterThan(0);
        RuleFor(x => x.DealDate).NotEmpty().LessThanOrEqualTo(DateTime.UtcNow.AddDays(1));
        RuleFor(x => x.DealDerType).GreaterThan(0);
        RuleFor(x => x.DealBusinessId).GreaterThan(0);
        RuleFor(x => x.DealModifiedBy).GreaterThan(0);
    }
}

public class CreateDealDetailCommandValidator : AbstractValidator<CreateDealDetailCommand>
{
    public CreateDealDetailCommandValidator()
    {
        RuleFor(x => x.DealId).GreaterThan(0);
        RuleFor(x => x.DealNo).GreaterThan(0);
        RuleFor(x => x.DealBatchId).GreaterThan(0);
        RuleFor(x => x.DealTranType)
            .Must(t => t == null || "BSPC".Contains(t.Value))
            .WithMessage("Transaction type must be B, S, P, or C.");
        RuleFor(x => x.DealAmount)
            .GreaterThan(0).When(x => x.DealAmount.HasValue);
        RuleFor(x => x.DealMatDate)
            .GreaterThan(DateTime.UtcNow).When(x => x.DealMatDate.HasValue)
            .WithMessage("Maturity date must be in the future.");
    }
}

public class ApproveDealCommandValidator : AbstractValidator<ApproveDealCommand>
{
    public ApproveDealCommandValidator()
    {
        RuleFor(x => x.DealId).GreaterThan(0);
        RuleFor(x => x.AppBusiness).GreaterThan(0);
        RuleFor(x => x.ModifiedBy).GreaterThan(0);
    }
}

public class CreateDealSettlementCommandValidator : AbstractValidator<CreateDealSettlementCommand>
{
    public CreateDealSettlementCommandValidator()
    {
        RuleFor(x => x.SetId).GreaterThan(0);
        RuleFor(x => x.DealId).GreaterThan(0);
        RuleFor(x => x.SetType)
            .Must(t => t == null || "UCR".Contains(t.Value))
            .WithMessage("Settlement type must be U (Utilized), C (Cancelled), or R (Rollover).");
    }
}
