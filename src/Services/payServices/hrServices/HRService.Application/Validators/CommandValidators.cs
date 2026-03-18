using FluentValidation;
using HRService.Application.Commands;

namespace HRService.Application.Validators;

public class CreateEmployeeCommandValidator : AbstractValidator<CreateEmployeeCommand>
{
    public CreateEmployeeCommandValidator()
    {
        RuleFor(x => x.EmployeeCode)
            .NotEmpty().WithMessage("Employee code is required")
            .MaximumLength(50).WithMessage("Employee code cannot exceed 50 characters");

        RuleFor(x => x.FirstName)
            .NotEmpty().WithMessage("First name is required")
            .MaximumLength(100).WithMessage("First name cannot exceed 100 characters");

        RuleFor(x => x.LastName)
            .NotEmpty().WithMessage("Last name is required")
            .MaximumLength(100).WithMessage("Last name cannot exceed 100 characters");

        RuleFor(x => x.Email)
            .NotEmpty().WithMessage("Email is required")
            .EmailAddress().WithMessage("Email format is invalid");

        RuleFor(x => x.DateOfBirth)
            .Must(d => DateTime.Today.AddYears(-18) >= d)
            .WithMessage("Employee must be at least 18 years old");

        RuleFor(x => x.DepartmentId)
            .NotEmpty().WithMessage("Department is required");

        RuleFor(x => x.PositionId)
            .NotEmpty().WithMessage("Position is required");

        RuleFor(x => x.JoinDate)
            .LessThanOrEqualTo(DateTime.Today)
            .WithMessage("Join date cannot be in the future");

        RuleFor(x => x.EmploymentType)
            .NotEmpty().WithMessage("Employment type is required")
            .Must(t => new[] { "Permanent", "Contract", "Probation" }.Contains(t))
            .WithMessage("Invalid employment type");
    }
}

public class RequestLeaveCommandValidator : AbstractValidator<RequestLeaveCommand>
{
    public RequestLeaveCommandValidator()
    {
        RuleFor(x => x.EmployeeId)
            .NotEmpty().WithMessage("Employee is required");

        RuleFor(x => x.LeaveTypeId)
            .NotEmpty().WithMessage("Leave type is required");

        RuleFor(x => x.StartDate)
            .GreaterThanOrEqualTo(DateTime.Today)
            .WithMessage("Start date cannot be in the past");

        RuleFor(x => x.EndDate)
            .GreaterThan(x => x.StartDate)
            .WithMessage("End date must be after start date");
    }
}

public class TerminateEmployeeCommandValidator : AbstractValidator<TerminateEmployeeCommand>
{
    public TerminateEmployeeCommandValidator()
    {
        RuleFor(x => x.EmployeeId)
            .NotEmpty().WithMessage("Employee is required");

        RuleFor(x => x.TerminationDate)
            .LessThanOrEqualTo(DateTime.Today)
            .WithMessage("Termination date cannot be in the future");

        RuleFor(x => x.Reason)
            .NotEmpty().WithMessage("Reason is required")
            .MaximumLength(500).WithMessage("Reason cannot exceed 500 characters");
    }
}
