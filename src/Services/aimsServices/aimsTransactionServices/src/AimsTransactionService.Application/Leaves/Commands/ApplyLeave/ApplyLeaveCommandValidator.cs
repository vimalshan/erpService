using FluentValidation;

namespace AimsTransactionService.Application.Leaves.Commands.ApplyLeave;

public sealed class ApplyLeaveCommandValidator : AbstractValidator<ApplyLeaveCommand>
{
    public ApplyLeaveCommandValidator()
    {
        RuleFor(x => x.EmployeeSysId)
            .GreaterThan(0).WithMessage("EmployeeSysId must be a valid ID.");

        RuleFor(x => x.LeaveId)
            .GreaterThan(0).WithMessage("LeaveId must be a valid leave type.");

        RuleFor(x => x.FromDate)
            .NotEmpty().WithMessage("FromDate is required.");

        RuleFor(x => x.ToDate)
            .NotEmpty().WithMessage("ToDate is required.")
            .GreaterThanOrEqualTo(x => x.FromDate).WithMessage("ToDate must be on or after FromDate.");

        RuleFor(x => x.LeaveDays)
            .GreaterThan(0).WithMessage("LeaveDays must be greater than zero.");

        RuleFor(x => x.AppliedBy)
            .GreaterThan(0).WithMessage("AppliedBy must be a valid user ID.");
    }
}
