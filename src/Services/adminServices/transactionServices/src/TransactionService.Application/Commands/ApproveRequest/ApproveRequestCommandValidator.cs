namespace TransactionService.Application.Commands.ApproveRequest;

using FluentValidation;

public sealed class ApproveRequestCommandValidator : AbstractValidator<ApproveRequestCommand>
{
    public ApproveRequestCommandValidator()
    {
        RuleFor(x => x.RequestSubId).GreaterThan(0);
        RuleFor(x => x.ApprovedQty).GreaterThanOrEqualTo(0);
        RuleFor(x => x.ApproverSysId).GreaterThan(0);
        RuleFor(x => x.Remarks).MaximumLength(255).When(x => x.Remarks is not null);
    }
}
