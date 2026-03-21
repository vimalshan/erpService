using FluentValidation;

namespace RackingSystem.Application.Features.Racks.Commands.CreateRack;

public sealed class CreateRackCommandValidator : AbstractValidator<CreateRackCommand>
{
    public CreateRackCommandValidator()
    {
        RuleFor(x => x.ZoneId).GreaterThan(0).WithMessage("ZoneId must be a positive integer.");
        RuleFor(x => x.Code).NotEmpty().MaximumLength(30).WithMessage("Code is required and must not exceed 30 characters.");
        RuleFor(x => x.RackType).MaximumLength(30).When(x => x.RackType != null);
        RuleFor(x => x.MaxLoadWeight).GreaterThan(0).When(x => x.MaxLoadWeight.HasValue)
            .WithMessage("MaxLoadWeight must be greater than zero.");
    }
}
