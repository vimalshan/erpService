using ConfigService.Application.Features.Currencies.Commands;
using FluentValidation;

namespace ConfigService.Application.Validators;

public class CreateCurrencyValidator : AbstractValidator<CreateCurrencyCommand>
{
    public CreateCurrencyValidator()
    {
        RuleFor(x => x.CurrencyCode).NotEmpty().MaximumLength(3);
    }
}

public class CreateVendorValidator : AbstractValidator<Features.Vendors.Commands.CreateVendorCommand>
{
    public CreateVendorValidator()
    {
        RuleFor(x => x.VendorId).NotEmpty();
        RuleFor(x => x.VendorName).NotEmpty().MaximumLength(255);
        RuleFor(x => x.VendorCode).NotEmpty().MaximumLength(255);
        RuleFor(x => x.EmailId).NotEmpty().MaximumLength(255);
    }
}

public class CreateCountryValidator : AbstractValidator<Features.Travel.Commands.CreateCountryCommand>
{
    public CreateCountryValidator()
    {
        RuleFor(x => x.CountryId).NotEmpty();
        RuleFor(x => x.CountryName).NotEmpty().MaximumLength(255);
        RuleFor(x => x.AirCode).NotEmpty().MaximumLength(255);
    }
}

public class CreateCityValidator : AbstractValidator<Features.Travel.Commands.CreateCityCommand>
{
    public CreateCityValidator()
    {
        RuleFor(x => x.CityId).NotEmpty();
        RuleFor(x => x.CountryId).NotEmpty();
        RuleFor(x => x.CityName).NotEmpty().MaximumLength(255);
        RuleFor(x => x.CityCode).NotEmpty().MaximumLength(255);
    }
}
