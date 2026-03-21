using FluentValidation;

namespace EnergyService.Application.Features.ProcessMail.Commands.ConfigureMailId;

public class ConfigureMailIdCommandValidator : AbstractValidator<ConfigureMailIdCommand>
{
    public ConfigureMailIdCommandValidator()
    {
        RuleFor(x => x.ProcessId).GreaterThan(0);
        RuleFor(x => x.MailId).NotEmpty().MaximumLength(65).EmailAddress();
        RuleFor(x => x.DeliveryType).NotEmpty().MaximumLength(3);
        RuleFor(x => x.StartDate).NotEmpty();
        RuleFor(x => x.ModifiedBy).GreaterThan(0);
    }
}
