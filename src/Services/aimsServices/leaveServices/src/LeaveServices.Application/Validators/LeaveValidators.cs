using FluentValidation;
using LeaveServices.Application.Commands.Leave;

namespace LeaveServices.Application.Validators;

public sealed class ApplyLeaveCommandValidator : AbstractValidator<ApplyLeaveCommand>
{
    public ApplyLeaveCommandValidator()
    {
        RuleFor(x => x.EmpSysId).GreaterThan(0).WithMessage("Employee ID is required.");
        RuleFor(x => x.LeaveId).GreaterThan(0).WithMessage("Leave type is required.");
        RuleFor(x => x.FromDate).LessThanOrEqualTo(x => x.ToDate).WithMessage("From date must be on or before To date.");
        RuleFor(x => x.AppliedDays).GreaterThan(0).WithMessage("Applied days must be greater than 0.");
        RuleFor(x => x.AppliedBy).GreaterThan(0).WithMessage("AppliedBy is required.");
        RuleFor(x => x.Reason).MaximumLength(500).When(x => x.Reason is not null);
        RuleFor(x => x.AppType).NotEmpty().MaximumLength(10);
    }
}

public sealed class ApproveLeaveCommandValidator : AbstractValidator<ApproveLeaveCommand>
{
    private static readonly string[] ValidStatuses = { "Y", "R", "C" };

    public ApproveLeaveCommandValidator()
    {
        RuleFor(x => x.LeaveDetailId).GreaterThan(0);
        RuleFor(x => x.Status).Must(s => ValidStatuses.Contains(s))
            .WithMessage("Status must be Y (Approved), R (Rejected), or C (Cancelled).");
        RuleFor(x => x.ApprovedBy).GreaterThan(0);
        RuleFor(x => x.Remarks).MaximumLength(500).When(x => x.Remarks is not null);
    }
}

public sealed class CreateLeaveMasterCommandValidator : AbstractValidator<CreateLeaveMasterCommand>
{
    public CreateLeaveMasterCommandValidator()
    {
        RuleFor(x => x.Description).NotEmpty().MaximumLength(255);
        RuleFor(x => x.MaxDaysPL).GreaterThanOrEqualTo(0);
        RuleFor(x => x.CreatedBy).GreaterThan(0);
    }
}
