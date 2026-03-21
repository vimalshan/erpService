using FluentValidation;

namespace RackingSystem.Application.Features.Racks.Commands.UpdateRack;

public sealed class UpdateRackCommandValidator : AbstractValidator<UpdateRackCommand>
{
    public UpdateRackCommandValidator()
    {
        RuleFor(x => x.Id).GreaterThan(0);
        RuleFor(x => x.Code).NotEmpty().MaximumLength(30);
        RuleFor(x => x.MaxLoadWeight).GreaterThan(0).When(x => x.MaxLoadWeight.HasValue);
    }
}
