using FluentValidation;

namespace ShipmentService.Application.Features.Shipments.Commands.UpdateShipmentStatus;

public sealed class UpdateShipmentStatusCommandValidator : AbstractValidator<UpdateShipmentStatusCommand>
{
    private static readonly string[] ValidStatuses =
        ["PENDING", "OPEN", "PICKED_UP", "IN_TRANSIT", "SHIPPED", "DELIVERED", "EXCEPTION", "CANCELLED"];

    public UpdateShipmentStatusCommandValidator()
    {
        RuleFor(x => x.ShipmentId).GreaterThan(0).WithMessage("Valid shipment ID is required.");
        RuleFor(x => x.NewStatus)
            .NotEmpty().WithMessage("New status is required.")
            .Must(s => ValidStatuses.Contains(s.ToUpper()))
            .WithMessage($"Status must be one of: {string.Join(", ", ValidStatuses)}");
    }
}
