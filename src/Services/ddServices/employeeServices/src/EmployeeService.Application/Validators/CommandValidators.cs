using EmployeeService.Application.Commands.Employees;
using FluentValidation;
using System;

namespace EmployeeService.Application.Validators
{
    /// <summary>
    /// Validator for CreateEmployeeCommand
    /// </summary>
    public class CreateEmployeeCommandValidator : AbstractValidator<CreateEmployeeCommand>
    {
        public CreateEmployeeCommandValidator()
        {
            RuleFor(x => x.FirstName)
                .NotEmpty().WithMessage("First name is required.")
                .MaximumLength(65).WithMessage("First name must not exceed 65 characters.");

            RuleFor(x => x.LastName)
                .NotEmpty().WithMessage("Last name is required.")
                .MaximumLength(65).WithMessage("Last name must not exceed 65 characters.");

            RuleFor(x => x.DateOfBirth)
                .NotEmpty().WithMessage("Date of birth is required.")
                .Must(dob => IsValidAge(dob)).WithMessage("Employee must be at least 18 years old.");

            RuleFor(x => x.Gender)
                .NotEmpty().WithMessage("Gender is required.")
                .Must(g => g == 'M' || g == 'F').WithMessage("Gender must be 'M' or 'F'.");

            RuleFor(x => x.Email)
                .NotEmpty().WithMessage("Email is required.")
                .EmailAddress().WithMessage("Email must be valid.");

            RuleFor(x => x.PhoneNumber)
                .NotEmpty().WithMessage("Phone number is required.")
                .Matches(@"^\d{10}$").WithMessage("Phone number must be 10 digits.");

            RuleFor(x => x.EmployeeNumber)
                .NotEmpty().WithMessage("Employee number is required.")
                .MaximumLength(20).WithMessage("Employee number must not exceed 20 characters.");

            RuleFor(x => x.UserId)
                .NotEmpty().WithMessage("User ID is required.")
                .MaximumLength(25).WithMessage("User ID must not exceed 25 characters.");

            RuleFor(x => x.JoiningDate)
                .NotEmpty().WithMessage("Joining date is required.")
                .LessThanOrEqualTo(DateTime.UtcNow).WithMessage("Joining date cannot be in the future.");

            RuleFor(x => x.GradeCode)
                .NotEmpty().WithMessage("Grade code is required.")
                .MaximumLength(3).WithMessage("Grade code must not exceed 3 characters.");

            RuleFor(x => x.Designation)
                .NotEmpty().WithMessage("Designation is required.")
                .MaximumLength(100).WithMessage("Designation must not exceed 100 characters.");

            RuleFor(x => x.BasicSalary)
                .GreaterThan(0).WithMessage("Basic salary must be greater than zero.");

            RuleFor(x => x.SalaryType)
                .NotEmpty().WithMessage("Salary type is required.")
                .MaximumLength(3).WithMessage("Salary type must not exceed 3 characters.");
        }

        private bool IsValidAge(DateTime dateOfBirth)
        {
            var age = DateTime.Today.Year - dateOfBirth.Year;
            if (dateOfBirth.Date > DateTime.Today.AddYears(-age)) age--;
            return age >= 18;
        }
    }

    /// <summary>
    /// Validator for UpdateEmployeePersonalInfoCommand
    /// </summary>
    public class UpdateEmployeePersonalInfoCommandValidator : AbstractValidator<UpdateEmployeePersonalInfoCommand>
    {
        public UpdateEmployeePersonalInfoCommandValidator()
        {
            RuleFor(x => x.EmployeeId)
                .GreaterThan(0).WithMessage("Employee ID must be greater than zero.");

            RuleFor(x => x.FirstName)
                .NotEmpty().WithMessage("First name is required.")
                .MaximumLength(65).WithMessage("First name must not exceed 65 characters.");

            RuleFor(x => x.LastName)
                .NotEmpty().WithMessage("Last name is required.")
                .MaximumLength(65).WithMessage("Last name must not exceed 65 characters.");

            RuleFor(x => x.DateOfBirth)
                .NotEmpty().WithMessage("Date of birth is required.")
                .Must(dob => IsValidAge(dob)).WithMessage("Employee must be at least 18 years old.");

            RuleFor(x => x.Gender)
                .NotEmpty().WithMessage("Gender is required.")
                .Must(g => g == 'M' || g == 'F').WithMessage("Gender must be 'M' or 'F'.");
        }

        private bool IsValidAge(DateTime dateOfBirth)
        {
            var age = DateTime.Today.Year - dateOfBirth.Year;
            if (dateOfBirth.Date > DateTime.Today.AddYears(-age)) age--;
            return age >= 18;
        }
    }

    /// <summary>
    /// Validator for UpdateEmployeeContactCommand
    /// </summary>
    public class UpdateEmployeeContactCommandValidator : AbstractValidator<UpdateEmployeeContactCommand>
    {
        public UpdateEmployeeContactCommandValidator()
        {
            RuleFor(x => x.EmployeeId)
                .GreaterThan(0).WithMessage("Employee ID must be greater than zero.");

            RuleFor(x => x.Email)
                .NotEmpty().WithMessage("Email is required.")
                .EmailAddress().WithMessage("Email must be valid.");

            RuleFor(x => x.PhoneNumber)
                .NotEmpty().WithMessage("Phone number is required.")
                .Matches(@"^\d{10}$").WithMessage("Phone number must be 10 digits.");
        }
    }

    /// <summary>
    /// Validator for TerminateEmployeeCommand
    /// </summary>
    public class TerminateEmployeeCommandValidator : AbstractValidator<TerminateEmployeeCommand>
    {
        public TerminateEmployeeCommandValidator()
        {
            RuleFor(x => x.EmployeeId)
                .GreaterThan(0).WithMessage("Employee ID must be greater than zero.");

            RuleFor(x => x.TerminationFlag)
                .NotEmpty().WithMessage("Termination flag is required.")
                .Must(f => f == "BLT" || f == "CLT").WithMessage("Termination flag must be 'BLT' or 'CLT'.");

            RuleFor(x => x.ExitDate)
                .NotEmpty().WithMessage("Exit date is required.")
                .LessThanOrEqualTo(DateTime.UtcNow).WithMessage("Exit date cannot be in the future.");
        }
    }

    /// <summary>
    /// Validator for PromoteEmployeeCommand
    /// </summary>
    public class PromoteEmployeeCommandValidator : AbstractValidator<PromoteEmployeeCommand>
    {
        public PromoteEmployeeCommandValidator()
        {
            RuleFor(x => x.EmployeeId)
                .GreaterThan(0).WithMessage("Employee ID must be greater than zero.");

            RuleFor(x => x.FromDesignation)
                .NotEmpty().WithMessage("Current designation is required.");

            RuleFor(x => x.ToDesignation)
                .NotEmpty().WithMessage("New designation is required.");

            RuleFor(x => x.NewBasicSalary)
                .GreaterThan(0).WithMessage("New basic salary must be greater than zero.");

            RuleFor(x => x.PromotionDate)
                .NotEmpty().WithMessage("Promotion date is required.")
                .LessThanOrEqualTo(DateTime.UtcNow).WithMessage("Promotion date cannot be in the future.");
        }
    }

    /// <summary>
    /// Validator for UpdateEmployeeSalaryCommand
    /// </summary>
    public class UpdateEmployeeSalaryCommandValidator : AbstractValidator<UpdateEmployeeSalaryCommand>
    {
        public UpdateEmployeeSalaryCommandValidator()
        {
            RuleFor(x => x.EmployeeId)
                .GreaterThan(0).WithMessage("Employee ID must be greater than zero.");

            RuleFor(x => x.BasicSalary)
                .GreaterThan(0).WithMessage("Basic salary must be greater than zero.");

            RuleFor(x => x.EffectiveDate)
                .NotEmpty().WithMessage("Effective date is required.");
        }
    }
}
