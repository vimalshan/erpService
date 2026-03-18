using System;
using EmployeeService.Application.Commands;
using FluentValidation;

namespace EmployeeService.Application.Validators;

public class CreateEmployeeCommandValidator : AbstractValidator<CreateEmployeeCommand>
{
    public CreateEmployeeCommandValidator()
    {
        RuleFor(x => x.EmployeeSystemId)
            .GreaterThan(0)
            .WithMessage("Employee System ID must be greater than 0");

        RuleFor(x => x.FirstName)
            .NotEmpty()
            .WithMessage("First Name is required")
            .MinimumLength(2)
            .WithMessage("First Name must be at least 2 characters long");

        RuleFor(x => x.LastName)
            .NotEmpty()
            .WithMessage("Last Name is required");

        RuleFor(x => x.Email)
            .NotEmpty()
            .WithMessage("Email is required")
            .EmailAddress()
            .WithMessage("Email must be a valid email address");

        RuleFor(x => x.EmployeeCode)
            .NotEmpty()
            .WithMessage("Employee Code is required");

        RuleFor(x => x.JoiningDate)
            .NotEmpty()
            .LessThanOrEqualTo(DateTime.UtcNow.Date)
            .WithMessage("Joining date cannot be in the future");

        RuleFor(x => x.GrossCTC)
            .GreaterThan(0)
            .WithMessage("Gross CTC must be greater than 0")
            .PrecisionScale(19, 0, true)
            .WithMessage("Gross CTC must be a valid decimal");

        RuleFor(x => x.BasicSalary)
            .GreaterThan(0)
            .WithMessage("Basic Salary must be greater than 0")
            .LessThanOrEqualTo(x => x.GrossCTC)
            .WithMessage("Basic Salary cannot exceed Gross CTC");

        RuleFor(x => x.CTCEffectiveDate)
            .NotEmpty()
            .WithMessage("CTC Effective Date is required");
    }
}

public class UpdateEmployeeCommandValidator : AbstractValidator<UpdateEmployeeCommand>
{
    public UpdateEmployeeCommandValidator()
    {
        RuleFor(x => x.EmployeeSystemId)
            .GreaterThan(0)
            .WithMessage("Employee System ID must be greater than 0");

        RuleFor(x => x.FirstName)
            .NotEmpty()
            .WithMessage("First Name is required")
            .MinimumLength(2)
            .WithMessage("First Name must be at least 2 characters long");

        RuleFor(x => x.LastName)
            .NotEmpty()
            .WithMessage("Last Name is required");

        RuleFor(x => x.Email)
            .NotEmpty()
            .WithMessage("Email is required")
            .EmailAddress()
            .WithMessage("Email must be a valid email address");
    }
}

public class ProcessSalaryIncrementCommandValidator : AbstractValidator<ProcessSalaryIncrementCommand>
{
    public ProcessSalaryIncrementCommandValidator()
    {
        RuleFor(x => x.EmployeeSystemId)
            .GreaterThan(0)
            .WithMessage("Employee System ID must be greater than 0");

        RuleFor(x => x.IncrementPercentage)
            .GreaterThan(0)
            .WithMessage("Increment Percentage must be greater than 0")
            .LessThanOrEqualTo(100)
            .WithMessage("Increment Percentage cannot exceed 100");

        RuleFor(x => x.EffectiveDate)
            .NotEmpty()
            .GreaterThanOrEqualTo(DateTime.UtcNow.Date)
            .WithMessage("Effective Date cannot be in the past");

        RuleFor(x => x.ApprovedBy)
            .GreaterThan(0)
            .WithMessage("ApprovedBy user ID must be greater than 0");
    }
}

public class ModifyEmployeeCTCCommandValidator : AbstractValidator<ModifyEmployeeCTCCommand>
{
    public ModifyEmployeeCTCCommandValidator()
    {
        RuleFor(x => x.EmployeeSystemId)
            .GreaterThan(0)
            .WithMessage("Employee System ID must be greater than 0");

        RuleFor(x => x.NewGrossCTC)
            .GreaterThan(0)
            .WithMessage("New Gross CTC must be greater than 0");

        RuleFor(x => x.NewBasicSalary)
            .GreaterThan(0)
            .WithMessage("New Basic Salary must be greater than 0")
            .LessThanOrEqualTo(x => x.NewGrossCTC)
            .WithMessage("Basic Salary cannot exceed Gross CTC");

        RuleFor(x => x.EffectiveDate)
            .NotEmpty()
            .WithMessage("Effective Date is required");

        RuleFor(x => x.Reason)
            .NotEmpty()
            .WithMessage("Reason for CTC modification is required");

        RuleFor(x => x.ModifiedBy)
            .GreaterThan(0)
            .WithMessage("ModifiedBy user ID must be greater than 0");
    }
}

public class TerminateEmployeeCommandValidator : AbstractValidator<TerminateEmployeeCommand>
{
    public TerminateEmployeeCommandValidator()
    {
        RuleFor(x => x.EmployeeSystemId)
            .GreaterThan(0)
            .WithMessage("Employee System ID must be greater than 0");

        RuleFor(x => x.TerminationDate)
            .NotEmpty()
            .GreaterThanOrEqualTo(DateTime.UtcNow.Date)
            .WithMessage("Termination Date cannot be in the past");
    }
}
