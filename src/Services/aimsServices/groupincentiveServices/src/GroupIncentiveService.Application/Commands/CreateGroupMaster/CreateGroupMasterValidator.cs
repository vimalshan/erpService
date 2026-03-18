using FluentValidation;

namespace GroupIncentiveService.Application.Commands.CreateGroupMaster;

public class CreateGroupMasterValidator : AbstractValidator<CreateGroupMasterCommand>
{
    public CreateGroupMasterValidator()
    {
        RuleFor(x => x.GroupName)
            .NotEmpty().WithMessage("Group name is required.")
            .MaximumLength(255).WithMessage("Group name must not exceed 255 characters.");

        RuleFor(x => x.GroupDescription)
            .MaximumLength(500).When(x => x.GroupDescription is not null)
            .WithMessage("Group description must not exceed 500 characters.");

        RuleFor(x => x.GroupEffDate)
            .NotEmpty().WithMessage("Effective date is required.");

        RuleFor(x => x.CreatedBy)
            .GreaterThan(0).WithMessage("CreatedBy must be a valid employee ID.");
    }
}
