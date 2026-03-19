using FluentValidation;
using MasterDataService.Application.Commands;

namespace MasterDataService.Application.Validators;

public class CreateLovMasterValidator : AbstractValidator<CreateLovMasterCommand>
{
    public CreateLovMasterValidator()
    {
        RuleFor(x => x.LovId).GreaterThan(0);
        RuleFor(x => x.LovType).NotEmpty().MaximumLength(10);
        RuleFor(x => x.LovName).NotEmpty().MaximumLength(200);
    }
}

public class CreateLovTypeMasterValidator : AbstractValidator<CreateLovTypeMasterCommand>
{
    public CreateLovTypeMasterValidator()
    {
        RuleFor(x => x.TypeCode).NotEmpty().MaximumLength(10);
        RuleFor(x => x.TypeName).NotEmpty().MaximumLength(50);
    }
}

public class CreateHoldTypeMasterValidator : AbstractValidator<CreateHoldTypeMasterCommand>
{
    public CreateHoldTypeMasterValidator()
    {
        RuleFor(x => x.HoldId).GreaterThan(0);
        RuleFor(x => x.HoldName).MaximumLength(100).When(x => x.HoldName is not null);
    }
}

public class CreateLocationScanParamValidator : AbstractValidator<CreateLocationScanParamCommand>
{
    public CreateLocationScanParamValidator()
    {
        RuleFor(x => x.ParamId).GreaterThan(0);
        RuleFor(x => x.LocationId).GreaterThan(0);
        RuleFor(x => x.EffectiveDate).NotEmpty();
        RuleFor(x => x.ClosingDate)
            .GreaterThan(x => x.EffectiveDate)
            .When(x => x.ClosingDate.HasValue)
            .WithMessage("Closing date must be after effective date.");
    }
}

public class CreateScannerMasterValidator : AbstractValidator<CreateScannerMasterCommand>
{
    public CreateScannerMasterValidator()
    {
        RuleFor(x => x.DeviceId).GreaterThan(0);
        RuleFor(x => x.DeviceName).MaximumLength(100).When(x => x.DeviceName is not null);
        RuleFor(x => x.DeviceLocationId).GreaterThan(0);
        RuleFor(x => x.DevicePath).MaximumLength(1000).When(x => x.DevicePath is not null);
    }
}
