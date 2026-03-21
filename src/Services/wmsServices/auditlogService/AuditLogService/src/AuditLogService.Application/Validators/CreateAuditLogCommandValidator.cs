using AuditLogService.Application.Commands;
using FluentValidation;

namespace AuditLogService.Application.Validators;

public class CreateAuditLogCommandValidator : AbstractValidator<CreateAuditLogCommand>
{
    public CreateAuditLogCommandValidator()
    {
        RuleFor(x => x.TableName)
            .NotEmpty().WithMessage("Table name is required.")
            .MaximumLength(100);

        RuleFor(x => x.RecordId)
            .GreaterThan(0).WithMessage("Record ID must be greater than 0.");

        RuleFor(x => x.Action)
            .NotEmpty().WithMessage("Action is required.")
            .Must(a => a is "INSERT" or "UPDATE" or "DELETE")
            .WithMessage("Action must be INSERT, UPDATE, or DELETE.");
    }
}
