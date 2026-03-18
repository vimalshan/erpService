using FluentValidation;

namespace GroupIncentiveService.Application.Commands.CreateGroupIncentive;

public class CreateGroupIncentiveValidator : AbstractValidator<CreateGroupIncentiveCommand>
{
    public CreateGroupIncentiveValidator()
    {
        RuleFor(x => x.GroupId).GreaterThan(0);
        RuleFor(x => x.Month).InclusiveBetween(1, 12);
        RuleFor(x => x.Year).InclusiveBetween(2000, 2100);
        RuleFor(x => x.TotalAmount).GreaterThanOrEqualTo(0);
        RuleFor(x => x.CreatedBy).GreaterThan(0);
        RuleFor(x => x.Details).NotEmpty().WithMessage("At least one detail record is required.");
        RuleForEach(x => x.Details).ChildRules(detail =>
        {
            detail.RuleFor(d => d.EmployeeId).GreaterThan(0);
            detail.RuleFor(d => d.AllocPercentage).InclusiveBetween(0.01m, 100m);
            detail.RuleFor(d => d.AllocAmount).GreaterThanOrEqualTo(0);
        });
        RuleFor(x => x.Details)
            .Must(d => d.Sum(x => x.AllocPercentage) <= 100)
            .WithMessage("Total allocation percentage must not exceed 100%.");
    }
}
