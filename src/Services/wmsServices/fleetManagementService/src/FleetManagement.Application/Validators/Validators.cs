using FleetManagement.Application.Commands.Vehicles;
using FleetManagement.Application.Commands.Drivers;
using FleetManagement.Application.Commands.Trips;
using FleetManagement.Application.Commands.Maintenance;
using FleetManagement.Application.Commands.Routes;
using FluentValidation;

namespace FleetManagement.Application.Validators;

public class CreateVehicleValidator : AbstractValidator<CreateVehicleCommand>
{
    public CreateVehicleValidator()
    {
        RuleFor(x => x.Code).NotEmpty().MaximumLength(30);
        RuleFor(x => x.LicensePlate).NotEmpty().MaximumLength(20);
        RuleFor(x => x.VehicleType).NotEmpty()
            .Must(t => new[] { "TRUCK", "FORKLIFT", "PALLET_JACK", "VAN", "OTHER" }.Contains(t))
            .WithMessage("Invalid vehicle type.");
    }
}

public class CreateDriverValidator : AbstractValidator<CreateDriverCommand>
{
    public CreateDriverValidator()
    {
        RuleFor(x => x.Code).NotEmpty().MaximumLength(30);
        RuleFor(x => x.FullName).NotEmpty().MaximumLength(100);
        RuleFor(x => x.LicenseNumber).NotEmpty().MaximumLength(50);
        RuleFor(x => x.LicenseExpiry).GreaterThan(DateTime.UtcNow).WithMessage("License must not be expired.");
        RuleFor(x => x.Email).EmailAddress().When(x => !string.IsNullOrEmpty(x.Email));
    }
}

public class CreateTripValidator : AbstractValidator<CreateTripCommand>
{
    public CreateTripValidator()
    {
        RuleFor(x => x.TripNumber).NotEmpty().MaximumLength(50);
        RuleFor(x => x.VehicleId).GreaterThan(0);
        RuleFor(x => x.DriverId).GreaterThan(0);
    }
}

public class CreateRouteValidator : AbstractValidator<CreateRouteCommand>
{
    public CreateRouteValidator()
    {
        RuleFor(x => x.RouteName).NotEmpty().MaximumLength(50);
        RuleFor(x => x.EstimatedDuration).GreaterThan(0).When(x => x.EstimatedDuration.HasValue);
    }
}

public class LogMaintenanceValidator : AbstractValidator<LogMaintenanceCommand>
{
    public LogMaintenanceValidator()
    {
        RuleFor(x => x.VehicleId).GreaterThan(0);
        RuleFor(x => x.MaintenanceType).NotEmpty().MaximumLength(50);
        RuleFor(x => x.Cost).GreaterThanOrEqualTo(0).When(x => x.Cost.HasValue);
    }
}

public class LogFuelValidator : AbstractValidator<LogFuelCommand>
{
    public LogFuelValidator()
    {
        RuleFor(x => x.VehicleId).GreaterThan(0);
        RuleFor(x => x.Gallons).GreaterThan(0);
        RuleFor(x => x.Cost).GreaterThanOrEqualTo(0);
    }
}
