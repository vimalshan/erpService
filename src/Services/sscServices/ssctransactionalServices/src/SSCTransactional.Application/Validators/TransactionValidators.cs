using FluentValidation;
using SSCTransactional.Application.Commands.Allocation;
using SSCTransactional.Application.Commands.Correspondence;
using SSCTransactional.Application.Commands.Approval;
using SSCTransactional.Application.Commands.Rescan;
using SSCTransactional.Application.Commands.Revoke;

namespace SSCTransactional.Application.Validators;

public class CreateAllocationValidator : AbstractValidator<CreateAllocationCommand>
{
    public CreateAllocationValidator()
    {
        RuleFor(x => x.DocId).GreaterThan(0);
        RuleFor(x => x.Action).NotEmpty().Must(a => a is "M" or "C" or "P")
            .WithMessage("Action must be M (Processing), C (Validation), or P (Payments)");
        RuleFor(x => x.GroupId).GreaterThan(0);
        RuleFor(x => x.Priority).GreaterThanOrEqualTo(0);
        RuleFor(x => x.AllocatedBy).GreaterThan(0);
    }
}

public class CreateCorrespondenceValidator : AbstractValidator<CreateCorrespondenceCommand>
{
    public CreateCorrespondenceValidator()
    {
        RuleFor(x => x.DocId).GreaterThan(0);
        RuleFor(x => x.AllocationId).GreaterThan(0);
        RuleFor(x => x.HoldCategory).GreaterThan(0);
        RuleFor(x => x.HoldType).GreaterThan(0);
        RuleFor(x => x.HoldRemarks).NotEmpty().MaximumLength(200);
        RuleFor(x => x.HoldBy).GreaterThan(0);
    }
}

public class CreateApprovalValidator : AbstractValidator<CreateApprovalCommand>
{
    public CreateApprovalValidator()
    {
        RuleFor(x => x.DocId).GreaterThan(0);
        RuleFor(x => x.ApproverUserId).GreaterThan(0);
        RuleFor(x => x.Status).NotEmpty().MaximumLength(1);
    }
}

public class CreateRescanValidator : AbstractValidator<CreateRescanCommand>
{
    public CreateRescanValidator()
    {
        RuleFor(x => x.DocId).GreaterThan(0);
        RuleFor(x => x.AllocationId).GreaterThan(0);
        RuleFor(x => x.RescanTo).NotEmpty().Must(t => t is "S" or "U")
            .WithMessage("RescanTo must be S (SSC) or U (User)");
        RuleFor(x => x.Remarks).NotEmpty().MaximumLength(100);
    }
}

public class CreateRevokeValidator : AbstractValidator<CreateRevokeCommand>
{
    public CreateRevokeValidator()
    {
        RuleFor(x => x.DocId).GreaterThan(0);
        RuleFor(x => x.Remarks).NotEmpty().MaximumLength(1000);
        RuleFor(x => x.Status).NotEmpty().MaximumLength(10);
        RuleFor(x => x.RevokedBy).GreaterThan(0);
    }
}
