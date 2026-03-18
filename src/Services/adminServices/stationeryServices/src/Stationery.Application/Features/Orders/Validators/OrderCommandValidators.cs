using FluentValidation;
using Stationery.Application.Features.Orders.Commands;

namespace Stationery.Application.Features.Orders.Validators;

public class CreateOrderCommandValidator : AbstractValidator<CreateOrderCommand>
{
    public CreateOrderCommandValidator()
    {
        RuleFor(v => v.LocationId).GreaterThan(0);
        RuleFor(v => v.VendorId).GreaterThan(0);
        RuleFor(v => v.OrderedBy).GreaterThan(0);
        RuleFor(v => v.DeliveryDate).GreaterThan(DateTime.UtcNow).WithMessage("Delivery date must be in the future.");
        RuleFor(v => v.Items).NotEmpty().WithMessage("Order must have at least one item.");
        RuleForEach(v => v.Items).ChildRules(item =>
        {
            item.RuleFor(i => i.RequestSubId).GreaterThan(0);
            item.RuleFor(i => i.OrderedQty).GreaterThan(0);
            item.RuleFor(i => i.OrderPrice).GreaterThanOrEqualTo(0);
        });
    }
}

public class ReceiveOrderCommandValidator : AbstractValidator<ReceiveOrderCommand>
{
    public ReceiveOrderCommandValidator()
    {
        RuleFor(v => v.OrderSubId).GreaterThan(0);
        RuleFor(v => v.ReceivedQty).GreaterThan(0);
        RuleFor(v => v.ReceivedBy).GreaterThan(0);
    }
}
