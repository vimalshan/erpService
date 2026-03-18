using AlertsNotifications.Application.Features.Alerts.Commands;
using FluentValidation;

namespace AlertsNotifications.Application.Validators;

public class CreateAlertCommandValidator : AbstractValidator<CreateAlertCommand>
{
    public CreateAlertCommandValidator()
    {
        RuleFor(x => x.AlertId).GreaterThan(0);
        RuleFor(x => x.AlertApps).NotEmpty().MaximumLength(10);
        RuleFor(x => x.AlertName).NotEmpty().MaximumLength(50);
        RuleFor(x => x.AlertType).NotEmpty().MaximumLength(10)
            .Must(t => t is "WD" or "WO" or "SD" or "SO")
            .WithMessage("Alert type must be WD, WO, SD, or SO.");
        RuleFor(x => x.AlertDesc).NotEmpty().MaximumLength(200);
        RuleFor(x => x.AlertToDesc).MaximumLength(200);
        RuleFor(x => x.AlertCcDesc).MaximumLength(200);
        RuleFor(x => x.AlertGradeCat).MaximumLength(3);
    }
}
