using FluentValidation;

namespace InventoryService.Application.Commands.ReceiveStock;

public class ReceiveStockCommandValidator : AbstractValidator<ReceiveStockCommand>
{
    public ReceiveStockCommandValidator()
    {
        RuleFor(x => x.ProductId).GreaterThan(0);
        RuleFor(x => x.WarehouseId).GreaterThan(0);
        RuleFor(x => x.BinId).GreaterThan(0);
        RuleFor(x => x.Quantity).GreaterThan(0);
    }
}
