using ContributionService.Application.Commands.ContributionBatch;
using ContributionService.Application.Commands.ContributionDetail;
using FluentValidation;

namespace ContributionService.Application.Behaviours;

public class CreateContributionBatchValidator : AbstractValidator<CreateContributionBatchCommand>
{
    public CreateContributionBatchValidator()
    {
        RuleFor(x => x.TrustCode).NotEmpty().MaximumLength(3);
        RuleFor(x => x.Category).NotEmpty().MaximumLength(3);
        RuleFor(x => x.PayunitCode).NotEmpty().MaximumLength(3);
        RuleFor(x => x.PayMonthStart).LessThanOrEqualTo(x => x.PayMonthEnd);
    }
}

public class ProcessMonthlyContributionValidator : AbstractValidator<ProcessMonthlyContributionCommand>
{
    public ProcessMonthlyContributionValidator()
    {
        RuleFor(x => x.MonthYear)
            .NotEmpty()
            .Matches(@"^\d{4}-\d{2}$").WithMessage("MonthYear must be in YYYY-MM format.");
        RuleFor(x => x.ProcessedByUserId).GreaterThan(0);
    }
}

public class CreateContributionDetailValidator : AbstractValidator<CreateContributionDetailCommand>
{
    public CreateContributionDetailValidator()
    {
        RuleFor(x => x.BatchNo).GreaterThan(0);
        RuleFor(x => x.MemberNo).GreaterThan(0);
        RuleFor(x => x.EmployeeNo).GreaterThan(0);
        RuleFor(x => x.BasicAmount).GreaterThanOrEqualTo(0);
        RuleFor(x => x.EeAmount).GreaterThanOrEqualTo(0);
        RuleFor(x => x.ErAmount).GreaterThanOrEqualTo(0);
        RuleFor(x => x.EntByUserId).NotEmpty().MaximumLength(255);
        RuleFor(x => x.TypeCode).NotEmpty().MaximumLength(1);
    }
}
