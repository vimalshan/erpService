using FluentValidation;

namespace PurchaseOrderService.Application.Commands.UpdatePurchaseOrder;

public class UpdatePurchaseOrderCommandValidator : AbstractValidator<UpdatePurchaseOrderCommand>
{
    public UpdatePurchaseOrderCommandValidator()
    {
        RuleFor(x => x.PoId).GreaterThan(0);
        RuleFor(x => x.Lines).NotEmpty();

        RuleForEach(x => x.Lines).ChildRules(line =>
        {
            line.RuleFor(l => l.ProductId).GreaterThan(0);
            line.RuleFor(l => l.LineNumber).GreaterThan(0);
            line.RuleFor(l => l.QuantityOrdered).GreaterThan(0);
            line.RuleFor(l => l.UnitPrice).GreaterThanOrEqualTo(0).When(l => l.UnitPrice.HasValue);
        });
    }
}
