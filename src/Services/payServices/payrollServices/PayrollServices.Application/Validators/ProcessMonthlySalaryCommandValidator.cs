using FluentValidation;
using PayrollServices.Application.Commands;

namespace PayrollServices.Application.Validators;

public class ProcessMonthlySalaryCommandValidator : AbstractValidator<ProcessMonthlySalaryCommand>
{
    public ProcessMonthlySalaryCommandValidator()
    {
        RuleFor(x => x.MonthYear)
            .NotEmpty()
            .Matches(@"^\d{4}-\d{2}$")
            .WithMessage("MonthYear must be in YYYY-MM format");

        RuleFor(x => x.ProcessedBy)
            .GreaterThan(0)
            .WithMessage("ProcessedBy must be a valid employee ID");
    }
}
