using FluentValidation;

namespace ClubMembershipService.Application.Commands.CreateMembership;

public class CreateMembershipCommandValidator : AbstractValidator<CreateMembershipCommand>
{
    public CreateMembershipCommandValidator()
    {
        RuleFor(x => x.ClubId).GreaterThan(0).WithMessage("ClubId must be valid.");
        RuleFor(x => x.MemberId).GreaterThan(0).WithMessage("MemberId must be valid.");
        RuleFor(x => x.EnrolledBy).GreaterThan(0).WithMessage("EnrolledBy must be a valid user ID.");
        RuleFor(x => x.MembershipFee)
            .GreaterThanOrEqualTo(0).When(x => x.MembershipFee.HasValue)
            .WithMessage("Membership fee cannot be negative.");
    }
}
