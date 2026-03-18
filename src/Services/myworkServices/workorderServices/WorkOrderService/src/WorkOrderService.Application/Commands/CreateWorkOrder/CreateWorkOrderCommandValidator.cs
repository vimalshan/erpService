using FluentValidation;

namespace WorkOrderService.Application.Commands.CreateWorkOrder;

public class CreateWorkOrderCommandValidator : AbstractValidator<CreateWorkOrderCommand>
{
    public CreateWorkOrderCommandValidator()
    {
        RuleFor(x => x.WorkOrderName)
            .NotEmpty().WithMessage("Work order name is required.")
            .MaximumLength(200).WithMessage("Work order name must not exceed 200 characters.");

        RuleFor(x => x.Description)
            .NotEmpty().WithMessage("Description is required.")
            .MaximumLength(500).WithMessage("Description must not exceed 500 characters.");

        RuleFor(x => x.DueDate)
            .GreaterThan(DateTime.Today).WithMessage("Due date must be in the future.");

        RuleFor(x => x.AssignedTo)
            .GreaterThan(0).WithMessage("AssignedTo must be a valid employee ID.");

        RuleFor(x => x.CreatedBy)
            .GreaterThan(0).WithMessage("CreatedBy must be a valid employee ID.");
    }
}
