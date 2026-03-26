using FluentValidation;

namespace AimsTransactionService.Application.Swipes.Commands.RecordSwipe;

public sealed class RecordSwipeCommandValidator : AbstractValidator<RecordSwipeCommand>
{
    public RecordSwipeCommandValidator()
    {
        RuleFor(x => x.EmployeeSysId)
            .GreaterThan(0).WithMessage("EmployeeSysId must be a valid ID.");

        RuleFor(x => x.GateNo)
            .GreaterThan(0).WithMessage("GateNo must be a valid gate number.");

        RuleFor(x => x.PunchTime)
            .NotEmpty().WithMessage("PunchTime is required.");

        RuleFor(x => x.PunchStatus)
            .Must(c => c is 'I' or 'O').WithMessage("PunchStatus must be I (In) or O (Out).");

        RuleFor(x => x.UpdatedBy)
            .GreaterThan(0).WithMessage("UpdatedBy must be a valid user ID.");
    }
}
