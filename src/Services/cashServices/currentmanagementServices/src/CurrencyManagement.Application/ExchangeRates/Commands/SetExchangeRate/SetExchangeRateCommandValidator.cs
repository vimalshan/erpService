using FluentValidation;

namespace CurrencyManagement.Application.ExchangeRates.Commands.SetExchangeRate;

/// <summary>
/// Validator for SetExchangeRateCommand
/// </summary>
public class SetExchangeRateCommandValidator : AbstractValidator<SetExchangeRateCommand>
{
    public SetExchangeRateCommandValidator()
    {
        RuleFor(x => x.RateId)
            .GreaterThan(0)
            .WithMessage("Rate ID must be positive");

        RuleFor(x => x.FinancialYear)
            .GreaterThan(0)
            .WithMessage("Financial year must be positive");

        RuleFor(x => x.Month)
            .InclusiveBetween(1, 12)
            .WithMessage("Month must be between 1 and 12");

        RuleFor(x => x.FromCurrencyId)
            .GreaterThan(0)
            .WithMessage("From Currency ID must be positive");

        RuleFor(x => x.ToCurrencyId)
            .GreaterThan(0)
            .WithMessage("To Currency ID must be positive");

        RuleFor(x => x.Rate)
            .GreaterThan(0)
            .WithMessage("Exchange rate must be greater than 0");

        RuleFor(x => x.ModifiedBy)
            .GreaterThan(0)
            .WithMessage("ModifiedBy must be a valid user ID");

        RuleFor(x => x)
            .Must(x => x.FromCurrencyId != x.ToCurrencyId)
            .WithMessage("From and To currencies cannot be the same");
    }
}
