using FluentValidation;

namespace MemberService.Application.Commands.CreateMember;

public class CreateMemberCommandValidator : AbstractValidator<CreateMemberCommand>
{
    public CreateMemberCommandValidator()
    {
        RuleFor(x => x.MemberName).NotEmpty().MaximumLength(65);
        RuleFor(x => x.TrustCode).NotEmpty().MaximumLength(3);
        RuleFor(x => x.DateOfJoining).NotEmpty().LessThanOrEqualTo(DateTime.UtcNow);
        RuleFor(x => x.EmployeeType).NotEmpty().Must(t => new[] { "N", "S", "O" }.Contains(t))
            .WithMessage("Employee type must be N (New), S (Transfer within SRF), or O (Transfer from Outside).");
        RuleFor(x => x.EmployeeSysId).GreaterThan(0);
        RuleFor(x => x.UnitCode).NotEmpty().MaximumLength(3);
        RuleFor(x => x.EmployeeNo).GreaterThan(0);
        RuleFor(x => x.CreatedBy).GreaterThan(0);
        RuleFor(x => x.DateOfBirth)
            .LessThan(x => x.DateOfJoining)
            .When(x => x.DateOfBirth.HasValue)
            .WithMessage("Date of birth must be before date of joining.");
    }
}
