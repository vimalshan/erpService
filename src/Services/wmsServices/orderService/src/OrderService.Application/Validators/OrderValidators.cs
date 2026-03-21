using FluentValidation;
using OrderService.Application.Commands;

namespace OrderService.Application.Validators;

public class CreateOrderCommandValidator : AbstractValidator<CreateOrderCommand>
{
    public CreateOrderCommandValidator()
    {
        RuleFor(x => x.Request.CustomerId).GreaterThan(0).WithMessage("CustomerId is required.");
        RuleFor(x => x.Request.Items).NotEmpty().WithMessage("At least one order item is required.");
        RuleForEach(x => x.Request.Items).ChildRules(item =>
        {
            item.RuleFor(i => i.ProductId).GreaterThan(0).WithMessage("ProductId is required.");
            item.RuleFor(i => i.Quantity).GreaterThan(0).WithMessage("Quantity must be greater than zero.");
            item.RuleFor(i => i.UnitPrice).GreaterThanOrEqualTo(0).WithMessage("UnitPrice cannot be negative.");
        });
    }
}

public class UpdateOrderStatusCommandValidator : AbstractValidator<UpdateOrderStatusCommand>
{
    private static readonly string[] ValidStatuses = { "PROCESSING", "SHIPPED", "CANCELLED" };

    public UpdateOrderStatusCommandValidator()
    {
        RuleFor(x => x.OrderId).GreaterThan(0).WithMessage("OrderId is required.");
        RuleFor(x => x.Status).Must(s => ValidStatuses.Contains(s.ToUpperInvariant()))
            .WithMessage("Status must be PROCESSING, SHIPPED, or CANCELLED.");
    }
}
