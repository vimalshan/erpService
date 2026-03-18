namespace ApprovalService.Application.Validation;

using FluentValidation;
using ApprovalService.Application.CQRS.Commands;
using ApprovalService.Application.DTOs;

/// <summary>
/// Validator for CreateApprovalMasterCommand
/// </summary>
public class CreateApprovalMasterValidator : AbstractValidator<CreateApprovalMasterCommand>
{
    public CreateApprovalMasterValidator()
    {
        RuleFor(x => x.Code)
            .NotEmpty().WithMessage("Code is required")
            .Length(1, 50).WithMessage("Code must be between 1 and 50 characters")
            .Matches(@"^[A-Z0-9_]+$").WithMessage("Code must contain only uppercase letters, numbers, and underscores");

        RuleFor(x => x.Name)
            .NotEmpty().WithMessage("Name is required")
            .Length(1, 255).WithMessage("Name must be between 1 and 255 characters");

        RuleFor(x => x.Module)
            .NotEmpty().WithMessage("Module is required")
            .Must(m => m == "PER" || m == "DDP" || m == "LET")
            .WithMessage("Module must be one of: PER, DDP, LET");

        RuleFor(x => x.Level)
            .GreaterThan(0).WithMessage("Level must be greater than 0")
            .LessThanOrEqualTo(10).WithMessage("Level must not exceed 10");

        RuleFor(x => x.UserId)
            .GreaterThan(0).WithMessage("UserId is required");
    }
}

/// <summary>
/// Validator for UpdateApprovalMasterCommand
/// </summary>
public class UpdateApprovalMasterValidator : AbstractValidator<UpdateApprovalMasterCommand>
{
    public UpdateApprovalMasterValidator()
    {
        RuleFor(x => x.Id)
            .GreaterThan(0).WithMessage("ID is required");

        RuleFor(x => x.Name)
            .NotEmpty().WithMessage("Name is required")
            .Length(1, 255).WithMessage("Name must be between 1 and 255 characters");

        RuleFor(x => x.Level)
            .GreaterThan(0).WithMessage("Level must be greater than 0")
            .LessThanOrEqualTo(10).WithMessage("Level must not exceed 10");

        RuleFor(x => x.UserId)
            .GreaterThan(0).WithMessage("UserId is required");
    }
}

/// <summary>
/// Validator for CreateApproverEmployeeCommand
/// </summary>
public class CreateApproverEmployeeValidator : AbstractValidator<CreateApproverEmployeeCommand>
{
    public CreateApproverEmployeeValidator()
    {
        RuleFor(x => x.ApprovalMasterId)
            .GreaterThan(0).WithMessage("ApprovalMasterId is required");

        RuleFor(x => x.EmployeeSysId)
            .GreaterThan(0).WithMessage("EmployeeSysId is required");

        RuleFor(x => x.ApproverLevel)
            .GreaterThan(0).WithMessage("ApproverLevel must be greater than 0")
            .LessThanOrEqualTo(10).WithMessage("ApproverLevel must not exceed 10");

        RuleFor(x => x.EffectiveFrom)
            .NotEmpty().WithMessage("EffectiveFrom is required");

        RuleFor(x => x.EffectiveTo)
            .GreaterThanOrEqualTo(x => x.EffectiveFrom)
            .When(x => x.EffectiveTo.HasValue)
            .WithMessage("EffectiveTo must be greater than or equal to EffectiveFrom");

        RuleFor(x => x.UserId)
            .GreaterThan(0).WithMessage("UserId is required");
    }
}

/// <summary>
/// Validator for CreateApprovalMasterDto
/// </summary>
public class CreateApprovalMasterDtoValidator : AbstractValidator<CreateApprovalMasterDto>
{
    public CreateApprovalMasterDtoValidator()
    {
        RuleFor(x => x.Code)
            .NotEmpty().WithMessage("Code is required")
            .Length(1, 50).WithMessage("Code must be between 1 and 50 characters");

        RuleFor(x => x.Name)
            .NotEmpty().WithMessage("Name is required")
            .Length(1, 255).WithMessage("Name must be between 1 and 255 characters");

        RuleFor(x => x.Module)
            .NotEmpty().WithMessage("Module is required")
            .Must(m => m == "PER" || m == "DDP" || m == "LET")
            .WithMessage("Module must be one of: PER, DDP, LET");
    }
}

/// <summary>
/// Validator for CreateApproverEmployeeDto
/// </summary>
public class CreateApproverEmployeeDtoValidator : AbstractValidator<CreateApproverEmployeeDto>
{
    public CreateApproverEmployeeDtoValidator()
    {
        RuleFor(x => x.ApprovalMasterId)
            .GreaterThan(0).WithMessage("ApprovalMasterId is required");

        RuleFor(x => x.EmployeeSysId)
            .GreaterThan(0).WithMessage("EmployeeSysId is required");

        RuleFor(x => x.ApproverLevel)
            .GreaterThan(0).WithMessage("ApproverLevel must be greater than 0");

        RuleFor(x => x.EffectiveFrom)
            .NotEmpty().WithMessage("EffectiveFrom is required")
            .LessThanOrEqualTo(DateTime.UtcNow)
            .WithMessage("EffectiveFrom cannot be in the future");

        RuleFor(x => x.EffectiveTo)
            .GreaterThanOrEqualTo(x => x.EffectiveFrom)
            .When(x => x.EffectiveTo.HasValue)
            .WithMessage("EffectiveTo must be greater than or equal to EffectiveFrom");
    }
}
