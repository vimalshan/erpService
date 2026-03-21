using FluentValidation;

namespace ReceivingService.Application.Commands.CreateReceiving;

public sealed class CreateReceivingCommandValidator
    : AbstractValidator<CreateReceivingCommand>
{
    public CreateReceivingCommandValidator()
    {
        RuleFor(x => x.ReceivingNumber)
            .NotEmpty().MaximumLength(50);

        RuleFor(x => x.PoId)
            .GreaterThan(0).WithMessage("A valid Purchase Order ID is required.");

        RuleFor(x => x.WarehouseId)
            .GreaterThan(0).WithMessage("A valid Warehouse ID is required.");

        RuleFor(x => x.Lines)
            .NotEmpty().WithMessage("At least one receiving line is required.");

        RuleForEach(x => x.Lines).ChildRules(line =>
        {
            line.RuleFor(l => l.PoLineId).GreaterThan(0);
            line.RuleFor(l => l.ProductId).GreaterThan(0);
            line.RuleFor(l => l.BinId).GreaterThan(0);
            line.RuleFor(l => l.QuantityReceived)
                .GreaterThan(0).WithMessage("Quantity received must be greater than zero.");
        });
    }
}
