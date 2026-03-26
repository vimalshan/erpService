using FluentValidation;

namespace AimsTransactionService.Application.Attendance.Commands.ProcessAttendanceBatch;

public sealed class ProcessAttendanceBatchCommandValidator : AbstractValidator<ProcessAttendanceBatchCommand>
{
    public ProcessAttendanceBatchCommandValidator()
    {
        RuleFor(x => x.MonthStart)
            .NotEmpty().WithMessage("MonthStart is required.");

        RuleFor(x => x.MonthEnd)
            .NotEmpty().WithMessage("MonthEnd is required.")
            .GreaterThanOrEqualTo(x => x.MonthStart).WithMessage("MonthEnd must be on or after MonthStart.");

        RuleFor(x => x.CreatedBy)
            .GreaterThan(0).WithMessage("CreatedBy must be a valid user ID.");
    }
}
