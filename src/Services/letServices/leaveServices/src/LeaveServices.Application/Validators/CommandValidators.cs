using FluentValidation;
using LeaveServices.Application.Features.LeaveEncashments.Commands;
using LeaveServices.Application.Features.LeaveRequests.Commands;
using LeaveServices.Application.Features.LossOfPay.Commands;

namespace LeaveServices.Application.Validators;

public class CreateLeaveRequestValidator : AbstractValidator<CreateLeaveRequestCommand>
{
    public CreateLeaveRequestValidator()
    {
        RuleFor(x => x.ReqNum).GreaterThan(0).WithMessage("Request number must be positive.");
        RuleFor(x => x.EmpUserId).NotEmpty().MaximumLength(25);
        RuleFor(x => x.SupUserId).MaximumLength(25).When(x => x.SupUserId is not null);
        RuleFor(x => x.FinyearSrlno).GreaterThan(0);
    }
}

public class ApplyLeaveEncashmentValidator : AbstractValidator<ApplyLeaveEncashmentCommand>
{
    public ApplyLeaveEncashmentValidator()
    {
        RuleFor(x => x.EmpSysId).GreaterThan(0);
        RuleFor(x => x.LeaveType).NotEmpty().MaximumLength(20);
        RuleFor(x => x.EncashmentDays).GreaterThan(0).LessThanOrEqualTo(90)
            .WithMessage("Encashment days must be between 1 and 90.");
        RuleFor(x => x.BasicSalary).GreaterThanOrEqualTo(0);
        RuleFor(x => x.RequestedBy).GreaterThan(0);
    }
}

public class UpdateEncashmentStatusValidator : AbstractValidator<UpdateEncashmentStatusCommand>
{
    private static readonly char[] ValidStatuses = ['P', 'A', 'R', 'D'];

    public UpdateEncashmentStatusValidator()
    {
        RuleFor(x => x.EncashmentId).GreaterThan(0);
        RuleFor(x => x.NewStatus).Must(s => ValidStatuses.Contains(s))
            .WithMessage("Status must be P (Pending), A (Approved), R (Rejected), or D (Processed).");
        RuleFor(x => x.ModifiedBy).GreaterThan(0);
    }
}

public class RecordLossOfPayValidator : AbstractValidator<RecordLossOfPayCommand>
{
    public RecordLossOfPayValidator()
    {
        RuleFor(x => x.EmpSysId).GreaterThan(0);
        RuleFor(x => x.LopDays).GreaterThan(0).LessThanOrEqualTo(31);
        RuleFor(x => x.Remarks).MaximumLength(500).When(x => x.Remarks is not null);
        RuleFor(x => x.RecordedBy).GreaterThan(0);
    }
}
