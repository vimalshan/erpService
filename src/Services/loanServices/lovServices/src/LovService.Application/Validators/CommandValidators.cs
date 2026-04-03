using FluentValidation;
using LovService.Application.Features.LovTypeMast.Commands;
using LovService.Application.Features.LovMaster.Commands;
using LovService.Application.Features.ProgramLovMast.Commands;

namespace LovService.Application.Validators;

public sealed class CreateLovTypeCommandValidator : AbstractValidator<CreateLovTypeCommand>
{
    public CreateLovTypeCommandValidator()
    {
        RuleFor(x => x.LovTypeId).GreaterThan(0);
        RuleFor(x => x.LovTypeName).NotEmpty().MaximumLength(100);
        RuleFor(x => x.LovCategory).Must(c => c == "F" || c == "V")
            .WithMessage("LovCategory must be 'F' (Fixed) or 'V' (Variable).");
        RuleFor(x => x.LovOrgId).GreaterThan(0);
    }
}

public sealed class UpdateLovTypeCommandValidator : AbstractValidator<UpdateLovTypeCommand>
{
    public UpdateLovTypeCommandValidator()
    {
        RuleFor(x => x.LovTypeId).GreaterThan(0);
        RuleFor(x => x.LovTypeName).NotEmpty().MaximumLength(100);
        RuleFor(x => x.LovCategory).Must(c => c == "F" || c == "V")
            .WithMessage("LovCategory must be 'F' (Fixed) or 'V' (Variable).");
        RuleFor(x => x.LovOrgId).GreaterThan(0);
    }
}

public sealed class CreateLovMasterCommandValidator : AbstractValidator<CreateLovMasterCommand>
{
    public CreateLovMasterCommandValidator()
    {
        RuleFor(x => x.LovTypeId).GreaterThan(0);
        RuleFor(x => x.LovName).NotEmpty().MaximumLength(65);
        RuleFor(x => x.CreatedBy).GreaterThan(0);
    }
}

public sealed class UpdateLovMasterCommandValidator : AbstractValidator<UpdateLovMasterCommand>
{
    public UpdateLovMasterCommandValidator()
    {
        RuleFor(x => x.LovId).GreaterThan(0);
        RuleFor(x => x.LovName).NotEmpty().MaximumLength(65);
        RuleFor(x => x.UpdatedBy).GreaterThan(0);
    }
}

public sealed class CreateProgramLovCommandValidator : AbstractValidator<CreateProgramLovCommand>
{
    public CreateProgramLovCommandValidator()
    {
        RuleFor(x => x.PrlovTypeCode).NotEmpty().MaximumLength(20);
        RuleFor(x => x.PrlovCode).NotEmpty().MaximumLength(5);
        RuleFor(x => x.PrlovName).NotEmpty().MaximumLength(200);
    }
}

public sealed class UpdateProgramLovCommandValidator : AbstractValidator<UpdateProgramLovCommand>
{
    public UpdateProgramLovCommandValidator()
    {
        RuleFor(x => x.PrlovTypeCode).NotEmpty().MaximumLength(20);
        RuleFor(x => x.PrlovCode).NotEmpty().MaximumLength(5);
        RuleFor(x => x.PrlovName).NotEmpty().MaximumLength(200);
    }
}
