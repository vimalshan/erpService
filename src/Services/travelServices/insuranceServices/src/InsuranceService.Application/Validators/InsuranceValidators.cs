using FluentValidation;
using InsuranceService.Application.Commands;

namespace InsuranceService.Application.Validators;

public class RegisterInsuranceCommandValidator : AbstractValidator<RegisterInsuranceCommand>
{
    public RegisterInsuranceCommandValidator()
    {
        RuleFor(x => x.CompanyCode).NotEmpty().MaximumLength(3);
        RuleFor(x => x.PlanNumber).GreaterThan(0);
        RuleFor(x => x.InsuranceType).NotEmpty().MaximumLength(3);
        RuleFor(x => x.PassportNumber).MaximumLength(50).When(x => x.PassportNumber != null);
        RuleFor(x => x.VisaPlace).MaximumLength(50).When(x => x.VisaPlace != null);
        RuleFor(x => x.Nominee1).MaximumLength(200).When(x => x.Nominee1 != null);
        RuleFor(x => x.Nominee2).MaximumLength(200).When(x => x.Nominee2 != null);
        RuleFor(x => x.Remarks).MaximumLength(200).When(x => x.Remarks != null);
    }
}

public class UpdateInsuranceStatusCommandValidator : AbstractValidator<UpdateInsuranceStatusCommand>
{
    public UpdateInsuranceStatusCommandValidator()
    {
        RuleFor(x => x.CompanyCode).NotEmpty().MaximumLength(3);
        RuleFor(x => x.PlanNumber).GreaterThan(0);
        RuleFor(x => x.Status).NotEmpty().Must(s => s is "A" or "I" or "E")
            .WithMessage("Status must be A (Active), I (Inactive), or E (Expired).");
        RuleFor(x => x.CertificateNumber).MaximumLength(200).When(x => x.CertificateNumber != null);
    }
}
