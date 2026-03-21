using FluentValidation;

namespace UnitService.Application.Commands.GrantAccess;

public class GrantAccessCommandValidator : AbstractValidator<GrantAccessCommand>
{
    public GrantAccessCommandValidator()
    {
        RuleFor(x => x.UnitCode).NotEmpty().MaximumLength(3);
        RuleFor(x => x.EmployeeSysId).GreaterThan(0);
        RuleFor(x => x.AccessType).NotEmpty().Must(x => x is "R" or "W" or "A")
            .WithMessage("AccessType must be R, W, or A.");
        RuleFor(x => x.Module).NotEmpty().MaximumLength(5);
        RuleFor(x => x.ModifiedBy).GreaterThan(0);
    }
}
