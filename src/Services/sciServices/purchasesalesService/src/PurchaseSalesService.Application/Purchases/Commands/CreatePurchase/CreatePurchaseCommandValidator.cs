using FluentValidation;

namespace PurchaseSalesService.Application.Purchases.Commands.CreatePurchase;

public sealed class CreatePurchaseCommandValidator : AbstractValidator<CreatePurchaseCommand>
{
    public CreatePurchaseCommandValidator()
    {
        RuleFor(x => x.TrackingNumber).GreaterThan(0);
        RuleFor(x => x.PurposeCode).GreaterThan(0);
        RuleFor(x => x.StageCode).GreaterThan(0);
        RuleFor(x => x.UserId).NotEmpty().MaximumLength(25);
        RuleFor(x => x.SupplierCode).MaximumLength(25).When(x => x.SupplierCode is not null);
    }
}
