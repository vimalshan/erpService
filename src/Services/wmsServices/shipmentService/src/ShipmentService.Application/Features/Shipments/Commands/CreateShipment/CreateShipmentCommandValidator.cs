using FluentValidation;

namespace ShipmentService.Application.Features.Shipments.Commands.CreateShipment;

public sealed class CreateShipmentCommandValidator : AbstractValidator<CreateShipmentCommand>
{
    public CreateShipmentCommandValidator()
    {
        RuleFor(x => x.ShipmentNumber)
            .NotEmpty().WithMessage("Shipment number is required.")
            .MaximumLength(50).WithMessage("Shipment number cannot exceed 50 characters.");

        RuleFor(x => x.CustomerId)
            .GreaterThan(0).WithMessage("Valid customer ID is required.");

        RuleFor(x => x.WarehouseId)
            .GreaterThan(0).WithMessage("Valid warehouse ID is required.");

        RuleFor(x => x.ShipmentType)
            .NotEmpty().WithMessage("Shipment type is required.")
            .Must(t => t is "INBOUND" or "OUTBOUND")
            .WithMessage("Shipment type must be INBOUND or OUTBOUND.");

        RuleFor(x => x.TrackingNumber)
            .MaximumLength(100).When(x => x.TrackingNumber is not null)
            .WithMessage("Tracking number cannot exceed 100 characters.");

        RuleFor(x => x.Carrier)
            .MaximumLength(50).When(x => x.Carrier is not null)
            .WithMessage("Carrier cannot exceed 50 characters.");
    }
}
