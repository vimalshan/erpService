using FluentValidation;
using Stationery.Application.Features.Requests.Commands;

namespace Stationery.Application.Features.Requests.Validators;

public class ApproveRequestCommandValidator : AbstractValidator<ApproveRequestCommand>
{
    public ApproveRequestCommandValidator()
    {
        RuleFor(v => v.RequestSubId).GreaterThan(0);
        RuleFor(v => v.ApprovedQty).GreaterThan(0).WithMessage("Approved quantity must be greater than zero.");
        RuleFor(v => v.ApproverSysId).GreaterThan(0);
        RuleFor(v => v.Remarks).MaximumLength(255).When(v => v.Remarks != null);
    }
}
