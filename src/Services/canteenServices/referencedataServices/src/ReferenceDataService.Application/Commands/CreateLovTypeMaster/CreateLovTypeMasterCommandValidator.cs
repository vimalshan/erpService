using FluentValidation;

namespace ReferenceDataService.Application.Commands.CreateLovTypeMaster;

public class CreateLovTypeMasterCommandValidator : AbstractValidator<CreateLovTypeMasterCommand>
{
    public CreateLovTypeMasterCommandValidator()
    {
        RuleFor(x => x.LovTypeCode)
            .NotEmpty().WithMessage("LovTypeCode is required.")
            .MaximumLength(3).WithMessage("LovTypeCode must be at most 3 characters.");

        RuleFor(x => x.LovTypeName)
            .MaximumLength(50).WithMessage("LovTypeName must be at most 50 characters.");
    }
}
