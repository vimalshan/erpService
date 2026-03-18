using FluentValidation;
using TaxService.Application.Commands;
using TaxService.Application.DTOs;

namespace TaxService.Application.Validators;

public class CreateTaxMarginalDetailCommandValidator : AbstractValidator<CreateTaxMarginalDetailCommand>
{
    public CreateTaxMarginalDetailCommandValidator()
    {
        RuleFor(x => x.Detail.EmployeeSystemId)
            .GreaterThan(0)
            .WithMessage("Employee System ID must be greater than 0");

        RuleFor(x => x.Detail.FinancialYear)
            .InclusiveBetween(2000, 2100)
            .WithMessage("Financial year must be between 2000 and 2100");

        RuleFor(x => x.Detail.GrossIncome)
            .GreaterThanOrEqualTo(0)
            .WithMessage("Gross income cannot be negative");

        RuleFor(x => x.Detail.StandardDeduction)
            .GreaterThanOrEqualTo(0)
            .WithMessage("Standard deduction cannot be negative");
    }
}

public class CreateConditionalMasterCommandValidator : AbstractValidator<CreateConditionalMasterCommand>
{
    public CreateConditionalMasterCommandValidator()
    {
        RuleFor(x => x.Master.PayeeId)
            .NotEmpty()
            .WithMessage("Payee ID is required");

        RuleFor(x => x.Master.PayeeName)
            .NotEmpty()
            .WithMessage("Payee name is required");

        RuleFor(x => x.Master.PayeeAddress)
            .NotEmpty()
            .WithMessage("Payee address is required");

        RuleFor(x => x.Master.TaxRegime)
            .Must(x => x == "Old" || x == "New")
            .WithMessage("Tax regime must be either 'Old' or 'New'");

        RuleFor(x => x.Master.FinancialYear)
            .InclusiveBetween(2000, 2100)
            .WithMessage("Financial year must be between 2000 and 2100");
    }
}

public class CreateTaxExemptionDtoValidator : AbstractValidator<CreateTaxExemptionDto>
{
    public CreateTaxExemptionDtoValidator()
    {
        RuleFor(x => x.ConditionalMasterId)
            .GreaterThan(0)
            .WithMessage("Conditional Master ID must be greater than 0");

        RuleFor(x => x.Code)
            .NotEmpty()
            .WithMessage("Exemption code is required");

        RuleFor(x => x.Amount)
            .GreaterThanOrEqualTo(0)
            .WithMessage("Amount cannot be negative");
    }
}

public class CreateTaxDeductionDtoValidator : AbstractValidator<CreateTaxDeductionDto>
{
    public CreateTaxDeductionDtoValidator()
    {
        RuleFor(x => x.ConditionalMasterId)
            .GreaterThan(0)
            .WithMessage("Conditional Master ID must be greater than 0");

        RuleFor(x => x.Code)
            .NotEmpty()
            .WithMessage("Deduction code is required");

        RuleFor(x => x.Amount)
            .GreaterThanOrEqualTo(0)
            .WithMessage("Amount cannot be negative");
    }
}
