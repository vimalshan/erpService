using FluentValidation;
using ProxyModule.Application.Commands.CreateProxyRight;

namespace ProxyModule.Application.Validators;

public class CreateProxyRightValidator : AbstractValidator<CreateProxyRightCommand>
{
    public CreateProxyRightValidator()
    {
        RuleFor(x => x.ProxyUserId).GreaterThan(0).WithMessage("Proxy user ID is required.");
        RuleFor(x => x.DelegatedUserId).GreaterThan(0).WithMessage("Delegated user ID is required.");
        RuleFor(x => x.ProxyUserId).NotEqual(x => x.DelegatedUserId).WithMessage("Proxy user and delegated user cannot be the same.");
        RuleFor(x => x.ProxyStartDate).NotEmpty().WithMessage("Start date is required.");
        RuleFor(x => x.ProxyEndDate).GreaterThan(x => x.ProxyStartDate)
            .When(x => x.ProxyEndDate.HasValue)
            .WithMessage("End date must be after start date.");
        RuleFor(x => x.ProxyType).NotEmpty().WithMessage("Proxy type is required.")
            .Must(BeValidProxyType).WithMessage("Invalid proxy type. Allowed: APPROVAL, SUBMISSION, FULL, READONLY.");
        RuleFor(x => x.CreatedBy).GreaterThan(0).WithMessage("Created by user ID is required.");
    }

    private static bool BeValidProxyType(string proxyType)
    {
        var validTypes = new[] { "APPROVAL", "SUBMISSION", "FULL", "READONLY" };
        return validTypes.Contains(proxyType?.ToUpperInvariant());
    }
}
