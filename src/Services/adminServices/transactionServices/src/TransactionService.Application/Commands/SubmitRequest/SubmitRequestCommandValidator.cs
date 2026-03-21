namespace TransactionService.Application.Commands.SubmitRequest;

using FluentValidation;

public sealed class SubmitRequestCommandValidator : AbstractValidator<SubmitRequestCommand>
{
    public SubmitRequestCommandValidator()
    {
        RuleFor(x => x.RequestedBy).GreaterThan(0).WithMessage("RequestedBy must be greater than 0.");
        RuleFor(x => x.LocationId).GreaterThan(0).WithMessage("LocationId must be greater than 0.");
        RuleFor(x => x.UnitCode).NotEmpty().MaximumLength(3).WithMessage("UnitCode is required (max 3 chars).");
        RuleFor(x => x.Items).NotEmpty().WithMessage("At least one item is required.");

        RuleForEach(x => x.Items).ChildRules(item =>
        {
            item.RuleFor(i => i.StationaryId).GreaterThan(0);
            item.RuleFor(i => i.DeptId).GreaterThan(0);
            item.RuleFor(i => i.ExpectedDate).GreaterThan(DateTime.UtcNow);
            item.RuleFor(i => i.RequestedQty).GreaterThan(0);
        });
    }
}
