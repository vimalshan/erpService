using FluentValidation;
using IntegrationService.Application.PurchaseOrders.Commands;

namespace IntegrationService.Application.Behaviours.Validators;

public class CreatePurchaseOrderValidator : AbstractValidator<CreatePurchaseOrderCommand>
{
    public CreatePurchaseOrderValidator()
    {
        RuleFor(x => x.PoSeqId).GreaterThan(0);
        RuleFor(x => x.OraclePoId).GreaterThan(0);
        RuleFor(x => x.PoNumber).NotEmpty().MaximumLength(25);
        RuleFor(x => x.VendorSiteId).GreaterThan(0);
        RuleFor(x => x.DueDays).GreaterThanOrEqualTo(0);
    }
}

public class CreateVendorValidator : AbstractValidator<Vendors.Commands.CreateVendorCommand>
{
    public CreateVendorValidator()
    {
        RuleFor(x => x.VendorId).GreaterThan(0);
        RuleFor(x => x.VendorName).NotEmpty().MaximumLength(200);
        RuleFor(x => x.VendorCode).NotEmpty().MaximumLength(200);
    }
}

public class CreateOrganizationUnitValidator : AbstractValidator<OrganizationUnits.Commands.CreateOrganizationUnitCommand>
{
    public CreateOrganizationUnitValidator()
    {
        RuleFor(x => x.OuId).NotEmpty().MaximumLength(25);
        RuleFor(x => x.OuName).NotEmpty().MaximumLength(250);
        RuleFor(x => x.BuId).NotEmpty().MaximumLength(25);
    }
}
