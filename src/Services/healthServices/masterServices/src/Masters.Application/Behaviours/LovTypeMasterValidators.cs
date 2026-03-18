using FluentValidation;
using Masters.Application.Commands;

namespace Masters.Application.Behaviours;

public class CreateLovTypeMasterCommandValidator : AbstractValidator<CreateLovTypeMasterCommand>
{
    public CreateLovTypeMasterCommandValidator()
    {
        RuleFor(x => x.LovTypeCode)
            .NotEmpty().WithMessage("LOV Type Code is required.")
            .Length(3).WithMessage("LOV Type Code must be exactly 3 characters.");

        RuleFor(x => x.LovTypeName)
            .NotEmpty().WithMessage("LOV Type Name is required.")
            .MaximumLength(50).WithMessage("LOV Type Name cannot exceed 50 characters.");
    }
}

public class UpdateLovTypeMasterCommandValidator : AbstractValidator<UpdateLovTypeMasterCommand>
{
    public UpdateLovTypeMasterCommandValidator()
    {
        RuleFor(x => x.LovTypeCode)
            .NotEmpty().WithMessage("LOV Type Code is required.")
            .Length(3).WithMessage("LOV Type Code must be exactly 3 characters.");

        RuleFor(x => x.LovTypeName)
            .NotEmpty().WithMessage("LOV Type Name is required.")
            .MaximumLength(50).WithMessage("LOV Type Name cannot exceed 50 characters.");
    }
}

public class DeleteLovTypeMasterCommandValidator : AbstractValidator<DeleteLovTypeMasterCommand>
{
    public DeleteLovTypeMasterCommandValidator()
    {
        RuleFor(x => x.LovTypeCode)
            .NotEmpty().WithMessage("LOV Type Code is required.")
            .Length(3).WithMessage("LOV Type Code must be exactly 3 characters.");
    }
}
