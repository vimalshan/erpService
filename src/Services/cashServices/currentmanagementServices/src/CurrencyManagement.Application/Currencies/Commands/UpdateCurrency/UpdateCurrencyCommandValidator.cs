using FluentValidation;

namespace CurrencyManagement.Application.Currencies.Commands.UpdateCurrency;

/// <summary>
/// Validator for UpdateCurrencyCommand
/// </summary>
public class UpdateCurrencyCommandValidator : AbstractValidator<UpdateCurrencyCommand>
{
    public UpdateCurrencyCommandValidator()
    {
        RuleFor(x => x.CurrencyId)
            .GreaterThan(0)
            .WithMessage("Currency ID must be positive");

        RuleFor(x => x.Name)
            .NotEmpty()
            .WithMessage("Currency name is required")
            .MaximumLength(255)
            .WithMessage("Currency name cannot exceed 255 characters");

        RuleFor(x => x.Symbol)
            .NotEmpty()
            .WithMessage("Currency symbol is required")
            .MaximumLength(25)
            .WithMessage("Currency symbol cannot exceed 25 characters");

        RuleFor(x => x.ModifiedBy)
            .GreaterThan(0)
            .WithMessage("ModifiedBy must be a valid user ID");
    }
}
