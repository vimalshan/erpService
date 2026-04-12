using FluentValidation;
using travelTransactionService.Application.Commands;

namespace travelTransactionService.Application.Validators;

public class CreateVendorCommandValidator : AbstractValidator<CreateVendorCommand>
{
    public CreateVendorCommandValidator()
    {
        RuleFor(x => x.VendorId).GreaterThan(0).WithMessage("Vendor ID must be greater than 0.");
        RuleFor(x => x.Name).NotEmpty().MaximumLength(65).WithMessage("Vendor name is required and max 65 chars.");
        RuleFor(x => x.CategoryType).NotEmpty().MaximumLength(1)
            .Must(x => x is "V" or "H").WithMessage("Category type must be 'V' (Vendor) or 'H' (Hotel).");
    }
}

public class UpdateVendorCommandValidator : AbstractValidator<UpdateVendorCommand>
{
    public UpdateVendorCommandValidator()
    {
        RuleFor(x => x.VendorId).GreaterThan(0);
        RuleFor(x => x.Name).NotEmpty().MaximumLength(65);
    }
}

public class CreateTaxMasterCommandValidator : AbstractValidator<CreateTaxMasterCommand>
{
    public CreateTaxMasterCommandValidator()
    {
        RuleFor(x => x.VendorId).GreaterThan(0);
        RuleFor(x => x.TaxType).NotEmpty().MaximumLength(5);
        RuleFor(x => x.TaxRate).GreaterThanOrEqualTo(0).When(x => x.TaxRate.HasValue);
    }
}

public class UpdateTaxRateCommandValidator : AbstractValidator<UpdateTaxRateCommand>
{
    public UpdateTaxRateCommandValidator()
    {
        RuleFor(x => x.TaxType).NotEmpty().MaximumLength(5);
        RuleFor(x => x.NewRate).GreaterThanOrEqualTo(0);
        RuleFor(x => x.ModifiedBy).GreaterThan(0);
    }
}

public class CreateJaiInterfaceLineCommandValidator : AbstractValidator<CreateJaiInterfaceLineCommand>
{
    public CreateJaiInterfaceLineCommandValidator()
    {
        RuleFor(x => x.OrgId).GreaterThan(0);
        RuleFor(x => x.PartyId).GreaterThan(0);
        RuleFor(x => x.PartySiteId).GreaterThan(0);
        RuleFor(x => x.ImportModule).NotEmpty().MaximumLength(255);
        RuleFor(x => x.TransactionNum).NotEmpty().MaximumLength(240);
        RuleFor(x => x.TransactionLineNum).GreaterThan(0);
    }
}

public class CreateTravelApParamsCommandValidator : AbstractValidator<CreateTravelApParamsCommand>
{
    public CreateTravelApParamsCommandValidator()
    {
        RuleFor(x => x.ApUnitId).GreaterThan(0);
        RuleFor(x => x.AccountStatus).NotEmpty().MaximumLength(1)
            .Must(x => x is "O" or "P").WithMessage("Account status must be 'O' (Official) or 'P' (Personal).");
        RuleFor(x => x.AccountCode).NotEmpty().MaximumLength(25);
    }
}
