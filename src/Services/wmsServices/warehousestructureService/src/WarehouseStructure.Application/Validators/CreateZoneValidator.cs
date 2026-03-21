using FluentValidation;
using WarehouseStructure.Application.Commands.CreateZone;

namespace WarehouseStructure.Application.Validators;

public sealed class CreateZoneValidator : AbstractValidator<CreateZoneCommand>
{
    private static readonly HashSet<string> ValidZoneTypes = new()
    {
        "RECEIVING", "STORAGE", "PICKING", "SHIPPING", "RETURNS", "PACKING"
    };

    public CreateZoneValidator()
    {
        RuleFor(x => x.Dto.WarehouseId)
            .GreaterThan(0).WithMessage("WarehouseId must be a positive integer.");

        RuleFor(x => x.Dto.Code)
            .NotEmpty().WithMessage("Code is required.")
            .MaximumLength(20).WithMessage("Code must not exceed 20 characters.");

        RuleFor(x => x.Dto.Name)
            .NotEmpty().WithMessage("Name is required.")
            .MaximumLength(100).WithMessage("Name must not exceed 100 characters.");

        RuleFor(x => x.Dto.ZoneType)
            .NotEmpty().WithMessage("ZoneType is required.")
            .Must(zt => ValidZoneTypes.Contains(zt.ToUpperInvariant()))
            .WithMessage($"ZoneType must be one of: {string.Join(", ", ValidZoneTypes)}");
    }
}
