using FluentValidation;
using PayTransactionalService.Application.Commands;

namespace PayTransactionalService.Application.Validators;

public class CreatePayTransactionCommandValidator : AbstractValidator<CreatePayTransactionCommand>
{
    public CreatePayTransactionCommandValidator()
    {
        RuleFor(x => x.Detail.EmployeeSystemId)
            .GreaterThan(0).WithMessage("Employee System ID must be greater than 0");
        RuleFor(x => x.Detail.MonthYear)
            .NotEmpty().WithMessage("MonthYear is required")
            .Matches(@"^\d{4}-\d{2}$").WithMessage("MonthYear must be in YYYY-MM format");
        RuleFor(x => x.Detail.GrossAmount)
            .GreaterThanOrEqualTo(0).WithMessage("Gross amount cannot be negative");
        RuleFor(x => x.Detail.Deductions)
            .GreaterThanOrEqualTo(0).WithMessage("Deductions cannot be negative");
    }
}

public class CreatePayArrearCommandValidator : AbstractValidator<CreatePayArrearCommand>
{
    public CreatePayArrearCommandValidator()
    {
        RuleFor(x => x.Detail.EmployeeSystemId)
            .GreaterThan(0).WithMessage("Employee System ID must be greater than 0");
        RuleFor(x => x.Detail.MonthYear)
            .NotEmpty().WithMessage("MonthYear is required")
            .Matches(@"^\d{4}-\d{2}$").WithMessage("MonthYear must be in YYYY-MM format");
        RuleFor(x => x.Detail.Type)
            .Must(t => t == "A" || t == "D").WithMessage("Type must be 'A' (Allowance) or 'D' (Deduction)");
        RuleFor(x => x.Detail.Amount)
            .NotEqual(0).WithMessage("Amount cannot be zero");
    }
}

public class CreatePayAdjustmentCommandValidator : AbstractValidator<CreatePayAdjustmentCommand>
{
    public CreatePayAdjustmentCommandValidator()
    {
        RuleFor(x => x.Detail.EmployeeSystemId)
            .GreaterThan(0).WithMessage("Employee System ID must be greater than 0");
        RuleFor(x => x.Detail.AdjustmentType)
            .NotEmpty().WithMessage("Adjustment type is required");
        RuleFor(x => x.Detail.MonthYear)
            .NotEmpty().WithMessage("MonthYear is required")
            .Matches(@"^\d{4}-\d{2}$").WithMessage("MonthYear must be in YYYY-MM format");
        RuleFor(x => x.Detail.EffectiveDate)
            .NotEmpty().WithMessage("Effective date is required");
    }
}

public class ProcessMonthlySalaryCommandValidator : AbstractValidator<ProcessMonthlySalaryCommand>
{
    public ProcessMonthlySalaryCommandValidator()
    {
        RuleFor(x => x.MonthYear)
            .NotEmpty().WithMessage("MonthYear is required")
            .Matches(@"^\d{4}-\d{2}$").WithMessage("MonthYear must be in YYYY-MM format");
    }
}
