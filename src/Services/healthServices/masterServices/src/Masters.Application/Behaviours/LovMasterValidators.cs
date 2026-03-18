using FluentValidation;
using Masters.Application.Commands;

namespace Masters.Application.Behaviours;

public class CreateLovMasterCommandValidator : AbstractValidator<CreateLovMasterCommand>
{
    public CreateLovMasterCommandValidator()
    {
        RuleFor(x => x.LovId)
            .GreaterThan(0).WithMessage("LOV ID must be greater than 0.");

        RuleFor(x => x.LovType)
            .NotEmpty().WithMessage("LOV Type is required.")
            .Length(3).WithMessage("LOV Type must be exactly 3 characters.");

        RuleFor(x => x.LovName)
            .NotEmpty().WithMessage("LOV Name is required.")
            .MaximumLength(2000).WithMessage("LOV Name cannot exceed 2000 characters.");
    }
}

public class UpdateLovMasterCommandValidator : AbstractValidator<UpdateLovMasterCommand>
{
    public UpdateLovMasterCommandValidator()
    {
        RuleFor(x => x.LovId)
            .GreaterThan(0).WithMessage("LOV ID must be greater than 0.");

        RuleFor(x => x.LovName)
            .NotEmpty().WithMessage("LOV Name is required.")
            .MaximumLength(2000).WithMessage("LOV Name cannot exceed 2000 characters.");
    }
}

public class DeleteLovMasterCommandValidator : AbstractValidator<DeleteLovMasterCommand>
{
    public DeleteLovMasterCommandValidator()
    {
        RuleFor(x => x.LovId)
            .GreaterThan(0).WithMessage("LOV ID must be greater than 0.");
    }
}
