using ExpenseService.Application.Commands;
using FluentValidation;

namespace ExpenseService.Application.Validators;

public class CalculateDACommandValidator : AbstractValidator<CalculateDACommand>
{
    public CalculateDACommandValidator()
    {
        RuleFor(x => x.RequestNumber).GreaterThan(0);
        RuleFor(x => x.FromDate).NotEmpty();
        RuleFor(x => x.ToDate).NotEmpty().GreaterThanOrEqualTo(x => x.FromDate)
            .WithMessage("End date must be after start date.");
        RuleFor(x => x.ArrangementType).NotEmpty().Must(x => x is "A" or "S")
            .WithMessage("Arrangement type must be 'A' (Admin) or 'S' (Self).");
        RuleFor(x => x.GradeCode).NotEmpty().MaximumLength(3);
    }
}
