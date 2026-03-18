using AlertsNotifications.Application.Features.Circulars.Commands;
using FluentValidation;

namespace AlertsNotifications.Application.Validators;

public class CreateCircularCommandValidator : AbstractValidator<CreateCircularCommand>
{
    public CreateCircularCommandValidator()
    {
        RuleFor(x => x.CircularId).GreaterThan(0);
        RuleFor(x => x.CircularYearId).GreaterThan(0);
        RuleFor(x => x.CircularType).GreaterThan(0);
        RuleFor(x => x.CircularOrgId).GreaterThan(0);
        RuleFor(x => x.CircularSignatoryId).GreaterThan(0);
        RuleFor(x => x.CircularDesc).NotEmpty().MaximumLength(4000);
        RuleFor(x => x.CircularSubject).NotEmpty().MaximumLength(1000);
        RuleFor(x => x.CircularToList).NotEmpty().MaximumLength(4000);
        RuleFor(x => x.CircularCcList).MaximumLength(4000);
        RuleFor(x => x.CircularStatus).Must(s => "DPARC".Contains(s))
            .WithMessage("Status must be D, P, A, R, or C.");
        RuleFor(x => x.CreatedBy).GreaterThan(0);
    }
}
