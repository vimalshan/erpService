using FluentValidation;

namespace CurrencyManagement.Application.OrganizationCurrencies.Commands.MapOrganizationCurrency;

/// <summary>
/// Validator for MapOrganizationCurrencyCommand
/// </summary>
public class MapOrganizationCurrencyCommandValidator : AbstractValidator<MapOrganizationCurrencyCommand>
{
    public MapOrganizationCurrencyCommandValidator()
    {
        RuleFor(x => x.OrganizationId)
            .GreaterThan(0)
            .WithMessage("Organization ID must be positive");

        RuleFor(x => x.CurrencyId)
            .GreaterThan(0)
            .WithMessage("Currency ID must be positive");

        RuleFor(x => x.ModifiedBy)
            .GreaterThan(0)
            .WithMessage("ModifiedBy must be a valid user ID");
    }
}
