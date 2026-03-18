using FluentValidation;
using DemandManagement.Application.Commands;

namespace DemandManagement.Application.Validators;

public class CreateDemandCommandValidator : AbstractValidator<CreateDemandCommand>
{
    public CreateDemandCommandValidator()
    {
        RuleFor(x => x.Request.DemandType)
            .NotEmpty().WithMessage("Demand type is required.")
            .MaximumLength(50);

        RuleFor(x => x.Request.DepartmentId)
            .GreaterThan(0).WithMessage("Department ID must be positive.");

        RuleFor(x => x.Request.DemandDescription)
            .NotEmpty().WithMessage("Description is required.")
            .MaximumLength(500);

        RuleFor(x => x.Request.Priority)
            .NotEmpty()
            .Must(p => new[] { "Low", "Medium", "High" }.Contains(p))
            .WithMessage("Priority must be Low, Medium, or High.");

        RuleFor(x => x.Request.CreatedBy)
            .GreaterThan(0).WithMessage("CreatedBy must be a valid user ID.");
    }
}
