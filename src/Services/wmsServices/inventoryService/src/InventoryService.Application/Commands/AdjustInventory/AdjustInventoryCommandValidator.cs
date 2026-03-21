using FluentValidation;

namespace InventoryService.Application.Commands.AdjustInventory;

public class AdjustInventoryCommandValidator : AbstractValidator<AdjustInventoryCommand>
{
    public AdjustInventoryCommandValidator()
    {
        RuleFor(x => x.ProductId).GreaterThan(0);
        RuleFor(x => x.WarehouseId).GreaterThan(0);
        RuleFor(x => x.BinId).GreaterThan(0);
        RuleFor(x => x.NewQuantity).GreaterThanOrEqualTo(0);
        RuleFor(x => x.Reason).NotEmpty().MaximumLength(255);
        RuleFor(x => x.AdjustedBy).NotEmpty().MaximumLength(50);
    }
}
