using FluentValidation;

namespace ClubMembershipService.Application.Commands.RecordActivity;

public class RecordActivityCommandValidator : AbstractValidator<RecordActivityCommand>
{
    public RecordActivityCommandValidator()
    {
        RuleFor(x => x.ClubId).GreaterThan(0).WithMessage("ClubId must be valid.");
        RuleFor(x => x.ActivityName)
            .NotEmpty().WithMessage("Activity name is required.")
            .MaximumLength(100).WithMessage("Activity name cannot exceed 100 characters.");
        RuleFor(x => x.OrganizerId).GreaterThan(0).WithMessage("OrganizerId must be valid.");
        RuleFor(x => x.Budget)
            .GreaterThanOrEqualTo(0).When(x => x.Budget.HasValue)
            .WithMessage("Budget cannot be negative.");
    }
}
