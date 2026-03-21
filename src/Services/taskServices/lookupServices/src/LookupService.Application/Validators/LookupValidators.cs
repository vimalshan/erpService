using FluentValidation;
using LookupService.Application.Commands;

namespace LookupService.Application.Validators;

public class CreateLovTypeValidator : AbstractValidator<CreateLovTypeCommand>
{
    public CreateLovTypeValidator()
    {
        RuleFor(x => x.LovTypeCode).NotEmpty().MaximumLength(3);
        RuleFor(x => x.LovTypeName).MaximumLength(50);
    }
}

public class CreateLovValidator : AbstractValidator<CreateLovCommand>
{
    public CreateLovValidator()
    {
        RuleFor(x => x.LovType).NotEmpty().MaximumLength(3);
        RuleFor(x => x.LovName).NotEmpty().MaximumLength(200);
    }
}

public class CreateProcessValidator : AbstractValidator<CreateProcessCommand>
{
    public CreateProcessValidator()
    {
        RuleFor(x => x.ProcessId).GreaterThan(0);
        RuleFor(x => x.ProcessName).NotEmpty().MaximumLength(50);
        RuleFor(x => x.LiveFlag).Must(x => x is "Y" or "N");
    }
}

public class MapLovToUnitValidator : AbstractValidator<MapLovToUnitCommand>
{
    public MapLovToUnitValidator()
    {
        RuleFor(x => x.LovId).GreaterThan(0);
        RuleFor(x => x.UnitCode).NotEmpty().MaximumLength(3);
        RuleFor(x => x.Flag).Must(x => x is "Y" or "N");
    }
}

public class MapUnitProcessValidator : AbstractValidator<MapUnitProcessCommand>
{
    public MapUnitProcessValidator()
    {
        RuleFor(x => x.UnitCode).NotEmpty().MaximumLength(3);
        RuleFor(x => x.ProcessId).GreaterThan(0);
    }
}

public class CreatePanelValidator : AbstractValidator<CreatePanelCommand>
{
    public CreatePanelValidator()
    {
        RuleFor(x => x.PanelId).GreaterThan(0);
        RuleFor(x => x.PanelName).NotEmpty().MaximumLength(65);
    }
}

public class CreateAccessMasterValidator : AbstractValidator<CreateAccessMasterCommand>
{
    public CreateAccessMasterValidator()
    {
        RuleFor(x => x.UnitLovMapId).GreaterThan(0);
        RuleFor(x => x.DepartmentId).GreaterThan(0);
        RuleFor(x => x.ProcessId).GreaterThan(0);
    }
}
