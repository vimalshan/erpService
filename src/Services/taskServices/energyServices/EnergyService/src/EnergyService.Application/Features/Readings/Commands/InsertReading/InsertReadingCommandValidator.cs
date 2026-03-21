using FluentValidation;

namespace EnergyService.Application.Features.Readings.Commands.InsertReading;

public class InsertReadingCommandValidator : AbstractValidator<InsertReadingCommand>
{
    public InsertReadingCommandValidator()
    {
        RuleFor(x => x.UnitCode).NotEmpty().MaximumLength(3);
        RuleFor(x => x.ProcessId).GreaterThan(0);
        RuleFor(x => x.ReadingValue).GreaterThanOrEqualTo(0);
        RuleFor(x => x.Remarks).MaximumLength(100).When(x => x.Remarks is not null);
        RuleFor(x => x.ModifiedBy).GreaterThan(0);
    }
}
