using FluentValidation;

namespace VisitorServices.Application.Visitors.Commands.RegisterVisitor;

public sealed class RegisterVisitorCommandValidator : AbstractValidator<RegisterVisitorCommand>
{
    public RegisterVisitorCommandValidator()
    {
        RuleFor(x => x.VisitorName)
            .NotEmpty().WithMessage("Visitor name is required.")
            .MaximumLength(255);

        RuleFor(x => x.IdType)
            .Must(c => "NPDO".Contains(c)).WithMessage("IdType must be N, P, D, or O.");

        RuleFor(x => x.Email)
            .EmailAddress().When(x => !string.IsNullOrEmpty(x.Email));

        RuleFor(x => x.PhoneNumber)
            .MaximumLength(20).When(x => !string.IsNullOrEmpty(x.PhoneNumber));

        RuleFor(x => x.WhomToVisit)
            .GreaterThan(0).WithMessage("WhomToVisit must be a valid employee ID.");

        RuleFor(x => x.EnteredBy)
            .GreaterThan(0).WithMessage("EnteredBy must be a valid user ID.");
    }
}
