using FluentValidation;

namespace WMTransactional.Application.Commands.CreateShipment;

public class CreateShipmentCommandValidator : AbstractValidator<CreateShipmentCommand>
{
    public CreateShipmentCommandValidator()
    {
        RuleFor(x => x.ShipmentNumber).NotEmpty().MaximumLength(50);
        RuleFor(x => x.SoId).GreaterThan(0);
        RuleFor(x => x.Lines).NotEmpty().WithMessage("At least one shipment line is required.");
        RuleForEach(x => x.Lines).ChildRules(line =>
        {
            line.RuleFor(l => l.SoLineId).GreaterThan(0);
            line.RuleFor(l => l.ProductId).GreaterThan(0);
            line.RuleFor(l => l.BinId).GreaterThan(0);
            line.RuleFor(l => l.QuantityShipped).GreaterThan(0);
        });
    }
}
