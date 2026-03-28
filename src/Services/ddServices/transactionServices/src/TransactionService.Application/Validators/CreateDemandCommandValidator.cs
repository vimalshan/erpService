using FluentValidation;
using TransactionService.Application.Commands;

namespace TransactionService.Application.Validators;

public class CreateDemandCommandValidator : AbstractValidator<CreateDemandCommand>
{
    public CreateDemandCommandValidator()
    {
        RuleFor(x => x.DemandType).NotEmpty().WithMessage("DemandType is required.");
        RuleFor(x => x.DepartmentId).GreaterThan(0).WithMessage("DepartmentId must be greater than 0.");
        RuleFor(x => x.DemandDescription).NotEmpty().WithMessage("DemandDescription is required.");
        RuleFor(x => x.RequiredDate).GreaterThan(DateTime.MinValue).WithMessage("RequiredDate is required.");
        RuleFor(x => x.Priority).NotEmpty().WithMessage("Priority is required.");
        RuleFor(x => x.CreatedBy).GreaterThan(0).WithMessage("CreatedBy must be greater than 0.");
    }
}
