using FluentValidation;

namespace InventoryService.Application.Commands.TransferInventory;

public class TransferInventoryCommandValidator : AbstractValidator<TransferInventoryCommand>
{
    public TransferInventoryCommandValidator()
    {
        RuleFor(x => x.ProductId).GreaterThan(0);
        RuleFor(x => x.FromWarehouseId).GreaterThan(0);
        RuleFor(x => x.FromBinId).GreaterThan(0);
        RuleFor(x => x.ToWarehouseId).GreaterThan(0);
        RuleFor(x => x.ToBinId).GreaterThan(0);
        RuleFor(x => x.Quantity).GreaterThan(0);
    }
}
