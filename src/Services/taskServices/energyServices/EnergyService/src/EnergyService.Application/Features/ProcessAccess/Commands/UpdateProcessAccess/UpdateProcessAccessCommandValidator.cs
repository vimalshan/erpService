using FluentValidation;

namespace EnergyService.Application.Features.ProcessAccess.Commands.UpdateProcessAccess;

public class UpdateProcessAccessCommandValidator : AbstractValidator<UpdateProcessAccessCommand>
{
    public UpdateProcessAccessCommandValidator()
    {
        RuleFor(x => x.ProcessId).GreaterThan(0);
        RuleFor(x => x.EmployeeSysId).GreaterThan(0);
        RuleFor(x => x.StartDate).NotEmpty();
        RuleFor(x => x.ModifiedBy).GreaterThan(0);
    }
}
