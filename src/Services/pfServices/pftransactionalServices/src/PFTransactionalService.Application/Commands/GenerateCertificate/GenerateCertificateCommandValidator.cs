using FluentValidation;

namespace PFTransactionalService.Application.Commands.GenerateCertificate;

public class GenerateCertificateCommandValidator : AbstractValidator<GenerateCertificateCommand>
{
    public GenerateCertificateCommandValidator()
    {
        RuleFor(x => x.SettlementId).GreaterThan(0);
        RuleFor(x => x.GeneratedBy).GreaterThan(0);
    }
}
