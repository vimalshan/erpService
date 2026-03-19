using FluentValidation;
using MobileAppManagement.Application.Commands;

namespace MobileAppManagement.Application.Validators;

public class RegisterDeviceCommandValidator : AbstractValidator<RegisterDeviceCommand>
{
    public RegisterDeviceCommandValidator()
    {
        RuleFor(x => x.EmployeeSysId).GreaterThan(0);
        RuleFor(x => x.DeviceId).NotEmpty().MaximumLength(200);
        RuleFor(x => x.DeviceType).Must(t => !string.IsNullOrEmpty(t) && new[] { "A", "I" }.Contains(t))
            .WithMessage("DeviceType must be 'A' (Android) or 'I' (iOS).");
        RuleFor(x => x.ImeiNo).MaximumLength(200);
        RuleFor(x => x.UpdatedBy).GreaterThan(0);
    }
}

public class DeactivateDeviceCommandValidator : AbstractValidator<DeactivateDeviceCommand>
{
    public DeactivateDeviceCommandValidator()
    {
        RuleFor(x => x.EmployeeSysId).GreaterThan(0);
        RuleFor(x => x.DeviceId).NotEmpty().MaximumLength(200);
        RuleFor(x => x.UpdatedBy).GreaterThan(0);
    }
}

public class LogUserLoginCommandValidator : AbstractValidator<LogUserLoginCommand>
{
    public LogUserLoginCommandValidator()
    {
        RuleFor(x => x.UserSysId).GreaterThan(0);
        RuleFor(x => x.DeviceType).Must(t => string.IsNullOrEmpty(t) || new[] { "A", "I" }.Contains(t))
            .WithMessage("DeviceType must be 'A' (Android) or 'I' (iOS).");
    }
}

public class CreateRegistrationCommandValidator : AbstractValidator<CreateRegistrationCommand>
{
    public CreateRegistrationCommandValidator()
    {
        RuleFor(x => x.RegistrationId).GreaterThan(0);
        RuleFor(x => x.UserId).MaximumLength(255);
        RuleFor(x => x.DeviceType).Must(t => string.IsNullOrEmpty(t) || new[] { "A", "I" }.Contains(t))
            .WithMessage("DeviceType must be 'A' (Android) or 'I' (iOS).");
        RuleFor(x => x.MobileNo).MaximumLength(255);
        RuleFor(x => x.ImeiNo).MaximumLength(255);
        RuleFor(x => x.DeviceId).MaximumLength(255);
    }
}

public class UpdateRegistrationStatusCommandValidator : AbstractValidator<UpdateRegistrationStatusCommand>
{
    public UpdateRegistrationStatusCommandValidator()
    {
        RuleFor(x => x.RegistrationId).GreaterThan(0);
        RuleFor(x => x.NewStatus).Must(s => !string.IsNullOrEmpty(s) && new[] { "P", "R", "C" }.Contains(s))
            .WithMessage("Status must be 'P' (Pending), 'R' (Registered), or 'C' (Closed).");
    }
}
