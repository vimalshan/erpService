using FluentValidation;
using ProxyModule.Application.Commands.UpdateProxyRight;

namespace ProxyModule.Application.Validators;

public class UpdateProxyRightValidator : AbstractValidator<UpdateProxyRightCommand>
{
    public UpdateProxyRightValidator()
    {
        RuleFor(x => x.ProxyId).GreaterThan(0).WithMessage("Proxy ID is required.");
        RuleFor(x => x.UpdatedBy).GreaterThan(0).WithMessage("Updated by user ID is required.");
        RuleFor(x => x.ProxyEndDate).GreaterThan(x => x.ProxyStartDate)
            .When(x => x.ProxyEndDate.HasValue && x.ProxyStartDate.HasValue)
            .WithMessage("End date must be after start date.");
        RuleFor(x => x.ProxyType)
            .Must(BeValidProxyType!)
            .When(x => !string.IsNullOrWhiteSpace(x.ProxyType))
            .WithMessage("Invalid proxy type. Allowed: APPROVAL, SUBMISSION, FULL, READONLY.");
    }

    private static bool BeValidProxyType(string proxyType)
    {
        var validTypes = new[] { "APPROVAL", "SUBMISSION", "FULL", "READONLY" };
        return validTypes.Contains(proxyType?.ToUpperInvariant());
    }
}
