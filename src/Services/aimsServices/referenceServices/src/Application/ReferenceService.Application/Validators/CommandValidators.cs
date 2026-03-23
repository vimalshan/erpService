using FluentValidation;
using ReferenceService.Application.Commands.LovType;
using ReferenceService.Application.Commands.LovValue;

namespace ReferenceService.Application.Validators;

/// <summary>
/// Validator for CreateLovTypeCommand.
/// </summary>
public class CreateLovTypeCommandValidator : AbstractValidator<CreateLovTypeCommand>
{
    public CreateLovTypeCommandValidator()
    {
        RuleFor(x => x.TypeName)
            .NotEmpty().WithMessage("Type name is required")
            .Length(1, 255).WithMessage("Type name must be between 1 and 255 characters");
        
        RuleFor(x => x.Sequence)
            .GreaterThanOrEqualTo(0).WithMessage("Sequence must be greater than or equal to 0");
        
        RuleFor(x => x.ModifiedBy)
            .GreaterThan(0).WithMessage("ModifiedBy must be a valid user ID");
    }
}

/// <summary>
/// Validator for CreateLovValueCommand.
/// </summary>
public class CreateLovValueCommandValidator : AbstractValidator<CreateLovValueCommand>
{
    public CreateLovValueCommandValidator()
    {
        RuleFor(x => x.Code)
            .NotEmpty().WithMessage("Code is required")
            .Length(1, 50).WithMessage("Code must be between 1 and 50 characters");
        
        RuleFor(x => x.Description)
            .NotEmpty().WithMessage("Description is required")
            .Length(1, 255).WithMessage("Description must be between 1 and 255 characters");
        
        RuleFor(x => x.Sequence)
            .GreaterThanOrEqualTo(0).WithMessage("Sequence must be greater than or equal to 0");
        
        RuleFor(x => x.TypeId)
            .GreaterThan(0).WithMessage("TypeId must be valid");
        
        RuleFor(x => x.ModifiedBy)
            .GreaterThan(0).WithMessage("ModifiedBy must be a valid user ID");
    }
}

/// <summary>
/// Validator for UpdateLovTypeCommand.
/// </summary>
public class UpdateLovTypeCommandValidator : AbstractValidator<UpdateLovTypeCommand>
{
    public UpdateLovTypeCommandValidator()
    {
        RuleFor(x => x.Id)
            .GreaterThan(0).WithMessage("ID must be valid");
        
        RuleFor(x => x.TypeName)
            .NotEmpty().WithMessage("Type name is required")
            .Length(1, 255).WithMessage("Type name must be between 1 and 255 characters");
        
        RuleFor(x => x.Sequence)
            .GreaterThanOrEqualTo(0).WithMessage("Sequence must be greater than or equal to 0");
    }
}

/// <summary>
/// Validator for UpdateLovValueCommand.
/// </summary>
public class UpdateLovValueCommandValidator : AbstractValidator<UpdateLovValueCommand>
{
    public UpdateLovValueCommandValidator()
    {
        RuleFor(x => x.Id)
            .GreaterThan(0).WithMessage("ID must be valid");
        
        RuleFor(x => x.Description)
            .NotEmpty().WithMessage("Description is required")
            .Length(1, 255).WithMessage("Description must be between 1 and 255 characters");
        
        RuleFor(x => x.Sequence)
            .GreaterThanOrEqualTo(0).WithMessage("Sequence must be greater than or equal to 0");
    }
}
