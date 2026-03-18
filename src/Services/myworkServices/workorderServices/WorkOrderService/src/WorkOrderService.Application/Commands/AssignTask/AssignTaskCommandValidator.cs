using FluentValidation;

namespace WorkOrderService.Application.Commands.AssignTask;

public class AssignTaskCommandValidator : AbstractValidator<AssignTaskCommand>
{
    public AssignTaskCommandValidator()
    {
        RuleFor(x => x.WorkOrderId)
            .GreaterThan(0).WithMessage("WorkOrderId must be a valid ID.");

        RuleFor(x => x.TaskName)
            .NotEmpty().WithMessage("Task name is required.")
            .MaximumLength(100).WithMessage("Task name must not exceed 100 characters.");

        RuleFor(x => x.AssignedTo)
            .GreaterThan(0).WithMessage("AssignedTo must be a valid employee ID.");

        RuleFor(x => x.EstimatedHours)
            .GreaterThan(0).WithMessage("EstimatedHours must be greater than 0.");

        RuleFor(x => x.CreatedBy)
            .GreaterThan(0).WithMessage("CreatedBy must be a valid employee ID.");
    }
}
