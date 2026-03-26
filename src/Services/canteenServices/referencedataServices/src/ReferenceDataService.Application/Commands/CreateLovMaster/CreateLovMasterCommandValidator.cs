using FluentValidation;

namespace ReferenceDataService.Application.Commands.CreateLovMaster;

public class CreateLovMasterCommandValidator : AbstractValidator<CreateLovMasterCommand>
{
    public CreateLovMasterCommandValidator()
    {
        RuleFor(x => x.LovId)
            .NotEmpty().WithMessage("LovId is required.")
            .MaximumLength(3).WithMessage("LovId must be at most 3 characters.");

        RuleFor(x => x.LovType)
            .MaximumLength(3).WithMessage("LovType must be at most 3 characters.");

        RuleFor(x => x.LovName)
            .MaximumLength(200).WithMessage("LovName must be at most 200 characters.");
    }
}
