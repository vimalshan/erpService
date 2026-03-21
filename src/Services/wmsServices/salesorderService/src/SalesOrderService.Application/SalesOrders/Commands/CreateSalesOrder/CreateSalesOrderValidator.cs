using FluentValidation;

namespace SalesOrderService.Application.SalesOrders.Commands.CreateSalesOrder;

public sealed class CreateSalesOrderValidator : AbstractValidator<CreateSalesOrderCommand>
{
    public CreateSalesOrderValidator()
    {
        RuleFor(x => x.SoNumber)
            .NotEmpty().MaximumLength(50);

        RuleFor(x => x.CustomerId)
            .GreaterThan(0).WithMessage("CustomerId must be positive.");

        RuleFor(x => x.WarehouseId)
            .GreaterThan(0).WithMessage("WarehouseId must be positive.");

        RuleFor(x => x.OrderDate)
            .NotEmpty();

        RuleFor(x => x.Lines)
            .NotEmpty().WithMessage("At least one order line is required.");

        RuleForEach(x => x.Lines).ChildRules(line =>
        {
            line.RuleFor(l => l.ProductId).GreaterThan(0);
            line.RuleFor(l => l.QuantityOrdered).GreaterThan(0);
            line.RuleFor(l => l.Discount).GreaterThanOrEqualTo(0);
        });
    }
}
