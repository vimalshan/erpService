using FluentValidation;

namespace ClubMembershipService.Application.Commands.CreateClub;

public class CreateClubCommandValidator : AbstractValidator<CreateClubCommand>
{
    public CreateClubCommandValidator()
    {
        RuleFor(x => x.ClubName)
            .NotEmpty().WithMessage("Club name is required.")
            .MaximumLength(100).WithMessage("Club name cannot exceed 100 characters.");

        RuleFor(x => x.CreatedBy)
            .GreaterThan(0).WithMessage("CreatedBy must be a valid user ID.");
    }
}
