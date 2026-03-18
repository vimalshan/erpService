using FluentValidation;
using AuditService.Application.Commands.Observations;

namespace AuditService.Application.Validators;

public class CreateObservationCommandValidator : AbstractValidator<CreateObservationCommand>
{
    private static readonly char[] ValidRisks = { 'A', 'B', 'C', 'D' };

    public CreateObservationCommandValidator()
    {
        RuleFor(x => x.ObvId).GreaterThan(0);
        RuleFor(x => x.AuditId).GreaterThan(0);
        RuleFor(x => x.Title).NotEmpty().MaximumLength(200);
        RuleFor(x => x.Description).NotEmpty().MaximumLength(2000);
        RuleFor(x => x.Risk).Must(r => ValidRisks.Contains(r)).WithMessage("Risk must be A, B, C, or D.");
        RuleFor(x => x.Auditee).GreaterThan(0);
        RuleFor(x => x.ManComments).NotEmpty().MaximumLength(2000);
        RuleFor(x => x.OrgDueDate).GreaterThan(DateTime.UtcNow).WithMessage("Due date must be in the future.");
        RuleFor(x => x.Location).NotEmpty().MaximumLength(200);
        RuleFor(x => x.AuditorName).NotEmpty().MaximumLength(200);
        RuleFor(x => x.CreatedBy).GreaterThan(0);
    }
}
