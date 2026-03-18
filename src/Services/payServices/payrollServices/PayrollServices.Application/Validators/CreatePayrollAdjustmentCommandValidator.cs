using FluentValidation;
using PayrollServices.Application.Commands;

namespace PayrollServices.Application.Validators;

public class CreatePayrollAdjustmentCommandValidator : AbstractValidator<CreatePayrollAdjustmentCommand>
{
    public CreatePayrollAdjustmentCommandValidator()
    {
        RuleFor(x => x.EmployeeSystemId)
            .GreaterThan(0)
            .WithMessage("EmployeeSystemId must be a valid ID");

        RuleFor(x => x.Amount)
            .GreaterThan(0)
            .WithMessage("Amount must be greater than 0");

        RuleFor(x => x.AdjustmentType)
            .NotEmpty()
            .Must(x => x == "A" || x == "D" || x == "R")
            .WithMessage("AdjustmentType must be A (Allowance), D (Deduction), or R (Arrear)");

        RuleFor(x => x.CreatedBy)
            .GreaterThan(0)
            .WithMessage("CreatedBy must be a valid employee ID");
    }
}
