using FluentValidation;

namespace InventoryManagement.Application.Commands.Items;

public sealed class RegisterItemCommandValidator : AbstractValidator<RegisterItemCommand>
{
    public RegisterItemCommandValidator()
    {
        RuleFor(x => x.OracleCode)
            .NotEmpty().WithMessage("Oracle code is required.")
            .MaximumLength(20).WithMessage("Oracle code cannot exceed 20 characters.");

        RuleFor(x => x.ItemName)
            .MaximumLength(100).WithMessage("Item name cannot exceed 100 characters.");

        RuleFor(x => x.ItemType)
            .NotEmpty().WithMessage("Item type is required.")
            .MaximumLength(20).WithMessage("Item type cannot exceed 20 characters.");

        RuleFor(x => x.ItemUomId).GreaterThan(0).WithMessage("Valid UOM is required.");
        RuleFor(x => x.ConversionFactor).GreaterThan(0).WithMessage("Conversion factor must be positive.");
    }
}
