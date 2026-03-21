using FluentValidation;

namespace ComplaintService.Application.Commands.UpdateAction;

public sealed class UpdateActionCommandValidator : AbstractValidator<UpdateActionCommand>
{
    private static readonly char[] ValidLevels = ['P', 'S', 'F', 'C'];

    public UpdateActionCommandValidator()
    {
        RuleFor(x => x.ActionNum).GreaterThan(0);
        RuleFor(x => x.ActionLevel).Must(l => ValidLevels.Contains(l))
            .WithMessage("ActionLevel must be P, S, F, or C.");
        RuleFor(x => x.Solution).NotEmpty().MaximumLength(4000);
        RuleFor(x => x.ActionBy).GreaterThan(0);
    }
}
