using FluentValidation;

namespace PurchaseSalesService.Application.Sales.Commands.CreateSale;

public sealed class CreateSaleCommandValidator : AbstractValidator<CreateSaleCommand>
{
    public CreateSaleCommandValidator()
    {
        RuleFor(x => x.TrackingNumber).GreaterThan(0);
        RuleFor(x => x.PurposeCode).GreaterThan(0);
        RuleFor(x => x.StageCode).GreaterThan(0);
        RuleFor(x => x.UserId).NotEmpty().MaximumLength(25);
        RuleFor(x => x.VehicleCustomer).MaximumLength(100).When(x => x.VehicleCustomer is not null);
    }
}
