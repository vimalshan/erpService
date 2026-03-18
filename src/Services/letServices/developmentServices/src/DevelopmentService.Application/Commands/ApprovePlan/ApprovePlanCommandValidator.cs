using FluentValidation;

namespace DevelopmentService.Application.Commands.ApprovePlan;

public class ApprovePlanCommandValidator : AbstractValidator<ApprovePlanCommand>
{
    private static readonly char[] ValidStatuses = ['A', 'R', 'B', 'F'];

    public ApprovePlanCommandValidator()
    {
        RuleFor(x => x.ReqNum).GreaterThan(0);
        RuleFor(x => x.AppStatus).Must(s => ValidStatuses.Contains(s))
            .WithMessage("AppStatus must be one of: A, R, B, F.");
    }
}
