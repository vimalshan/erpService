using FluentValidation;

namespace PurchaseOrderService.Application.Commands.ReceivePurchaseOrderLine;

public class ReceivePurchaseOrderLineCommandValidator : AbstractValidator<ReceivePurchaseOrderLineCommand>
{
    public ReceivePurchaseOrderLineCommandValidator()
    {
        RuleFor(x => x.PoId).GreaterThan(0);
        RuleFor(x => x.LineNumber).GreaterThan(0);
        RuleFor(x => x.Quantity).GreaterThan(0);
    }
}
