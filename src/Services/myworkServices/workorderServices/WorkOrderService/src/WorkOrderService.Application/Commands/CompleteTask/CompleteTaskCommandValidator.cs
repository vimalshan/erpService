using FluentValidation;

namespace WorkOrderService.Application.Commands.CompleteTask;

public class CompleteTaskCommandValidator : AbstractValidator<CompleteTaskCommand>
{
    public CompleteTaskCommandValidator()
    {
        RuleFor(x => x.TaskId)
            .GreaterThan(0).WithMessage("TaskId must be a valid ID.");

        RuleFor(x => x.ActualHours)
            .GreaterThanOrEqualTo(0).WithMessage("ActualHours must be 0 or greater.");

        RuleFor(x => x.CompletionRemarks)
            .MaximumLength(500).WithMessage("CompletionRemarks must not exceed 500 characters.");

        RuleFor(x => x.CompletedBy)
            .GreaterThan(0).WithMessage("CompletedBy must be a valid employee ID.");
    }
}
