using FluentValidation;

namespace RequestServices.Application.Commands.ApproveRequest;

public class ApproveRequestCommandValidator : AbstractValidator<ApproveRequestCommand>
{
    public ApproveRequestCommandValidator()
    {
        RuleFor(x => x.RequestId)      .GreaterThan(0);
        RuleFor(x => x.SerialNumber)   .GreaterThan(0);
        RuleFor(x => x.ApprovalNumber) .GreaterThan(0);
        RuleFor(x => x.ApprovalRemark) .NotEmpty().MaximumLength(200);
        RuleFor(x => x.ApprovalUser)   .NotEmpty().MaximumLength(20);
    }
}
