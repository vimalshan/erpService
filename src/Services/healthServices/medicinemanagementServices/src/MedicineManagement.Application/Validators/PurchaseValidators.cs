using FluentValidation;
using MedicineManagement.Application.Features.Purchases.Commands;

namespace MedicineManagement.Application.Validators;

public class CreatePurchaseValidator : AbstractValidator<CreatePurchaseCommand>
{
    public CreatePurchaseValidator()
    {
        RuleFor(x => x.CompanyCode).NotEmpty().MaximumLength(3);
        RuleFor(x => x.VendorName).NotEmpty().MaximumLength(100);
        RuleFor(x => x.InvoiceNumber).NotEmpty().MaximumLength(30);
        RuleFor(x => x.InvoiceAmount).GreaterThan(0);
        RuleFor(x => x.LineItems).NotEmpty().WithMessage("At least one line item is required.");
        RuleForEach(x => x.LineItems).ChildRules(item =>
        {
            item.RuleFor(i => i.MedicineCode).NotEmpty().MaximumLength(3);
            item.RuleFor(i => i.PackagingType).NotEmpty().MaximumLength(3);
            item.RuleFor(i => i.SerialNumber).NotEmpty();
        });
    }
}
