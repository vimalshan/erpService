using FluentValidation;
using VehicleTracking.Application.Vehicles.Commands;

namespace VehicleTracking.Application.Vehicles.Validators;

public class RegisterVehicleValidator : AbstractValidator<RegisterVehicleCommand>
{
    public RegisterVehicleValidator()
    {
        RuleFor(x => x.RegNum1).NotEmpty().MaximumLength(3);
        RuleFor(x => x.RegNum2).MaximumLength(2);
        RuleFor(x => x.RegNum3).MaximumLength(2);
        RuleFor(x => x.RegNum4).NotEmpty().MaximumLength(4);
        RuleFor(x => x.UpdatedBy).NotEmpty().MaximumLength(25);
    }
}

public class UpdateVehicleStageValidator : AbstractValidator<UpdateVehicleStageCommand>
{
    public UpdateVehicleStageValidator()
    {
        RuleFor(x => x.TrackingNumber).GreaterThan(0);
        RuleFor(x => x.StageCode).GreaterThan(0);
        RuleFor(x => x.EnteredBy).NotEmpty().MaximumLength(25);
    }
}

public class CreateVehicleTransactionValidator : AbstractValidator<CreateVehicleTransactionCommand>
{
    public CreateVehicleTransactionValidator()
    {
        RuleFor(x => x.LogEntryUser).NotEmpty().MaximumLength(255);
        RuleFor(x => x.PartyName).MaximumLength(200);
        RuleFor(x => x.DriverName).MaximumLength(100);
        RuleFor(x => x.DriverCell).MaximumLength(15);
        RuleFor(x => x.GateName).MaximumLength(25);
    }
}

public class CreateVehicleInvoiceValidator : AbstractValidator<CreateVehicleInvoiceCommand>
{
    public CreateVehicleInvoiceValidator()
    {
        RuleFor(x => x.TrackingNumber).GreaterThan(0);
        RuleFor(x => x.ReferenceNumber).GreaterThan(0);
        RuleFor(x => x.ModifiedUser).NotEmpty().MaximumLength(25);
    }
}

public class MakeDecisionValidator : AbstractValidator<MakeDecisionCommand>
{
    public MakeDecisionValidator()
    {
        RuleFor(x => x.TrackingNumber).GreaterThan(0);
        RuleFor(x => x.PurposeCode).GreaterThan(0);
        RuleFor(x => x.StageCode).GreaterThan(0);
        RuleFor(x => x.Remark).MaximumLength(100);
    }
}
