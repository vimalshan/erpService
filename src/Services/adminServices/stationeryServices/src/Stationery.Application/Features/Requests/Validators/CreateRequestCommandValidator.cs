using FluentValidation;
using Stationery.Application.Features.Requests.Commands;

namespace Stationery.Application.Features.Requests.Validators;

public class CreateRequestCommandValidator : AbstractValidator<CreateRequestCommand>
{
    public CreateRequestCommandValidator()
    {
        RuleFor(v => v.RequestedBy).GreaterThan(0);
        RuleFor(v => v.LocationId).GreaterThan(0);
        RuleFor(v => v.UnitCode).NotEmpty().MaximumLength(3);
        RuleFor(v => v.Details).NotEmpty().WithMessage("Request must have at least one item.");
        
        RuleForEach(v => v.Details).ChildRules(detail =>
        {
            detail.RuleFor(d => d.StationaryId).GreaterThan(0);
            detail.RuleFor(d => d.RequestedQty).GreaterThan(0);
            detail.RuleFor(d => d.DeptId).GreaterThan(0);
        });
    }
}
