using FluentValidation;

namespace EnergyService.Application.Features.Processes.Commands.CreateProcess;

public class CreateProcessCommandValidator : AbstractValidator<CreateProcessCommand>
{
    public CreateProcessCommandValidator()
    {
        RuleFor(x => x.EcProcessId).GreaterThan(0);
        RuleFor(x => x.EcProcessDesc).NotEmpty().MaximumLength(65);
        RuleFor(x => x.EcUnitCode).NotEmpty().MaximumLength(3);
        RuleFor(x => x.EcCloseFlag).NotEmpty().MaximumLength(1);
        RuleFor(x => x.ModifiedBy).GreaterThan(0);
    }
}
