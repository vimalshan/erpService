namespace TransactionService.Application.Commands.CreateOrder;

using FluentValidation;

public sealed class CreateOrderCommandValidator : AbstractValidator<CreateOrderCommand>
{
    public CreateOrderCommandValidator()
    {
        RuleFor(x => x.LocationId).GreaterThan(0);
        RuleFor(x => x.VendorId).GreaterThan(0);
        RuleFor(x => x.DeliveryDate).GreaterThan(DateTime.UtcNow);
        RuleFor(x => x.OrderedBy).GreaterThan(0);
        RuleFor(x => x.Items).NotEmpty().WithMessage("At least one order item is required.");

        RuleForEach(x => x.Items).ChildRules(item =>
        {
            item.RuleFor(i => i.RequestSubId).GreaterThan(0);
            item.RuleFor(i => i.OrderedQty).GreaterThan(0);
            item.RuleFor(i => i.OrderPrice).GreaterThanOrEqualTo(0);
        });
    }
}
