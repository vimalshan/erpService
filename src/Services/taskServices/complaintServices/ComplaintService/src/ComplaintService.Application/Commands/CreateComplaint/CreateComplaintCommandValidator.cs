using FluentValidation;

namespace ComplaintService.Application.Commands.CreateComplaint;

public sealed class CreateComplaintCommandValidator : AbstractValidator<CreateComplaintCommand>
{
    public CreateComplaintCommandValidator()
    {
        RuleFor(x => x.GroupId).GreaterThan(0).WithMessage("GroupId must be positive.");
        RuleFor(x => x.Type).GreaterThan(0).WithMessage("Type must be positive.");
        RuleFor(x => x.Location).GreaterThan(0).WithMessage("Location must be positive.");
        RuleFor(x => x.Department).GreaterThan(0).WithMessage("Department must be positive.");
        RuleFor(x => x.Process).GreaterThan(0).WithMessage("Process must be positive.");
        RuleFor(x => x.Subject).MaximumLength(500).When(x => x.Subject != null);
        RuleFor(x => x.Description).MaximumLength(4000).When(x => x.Description != null);
        RuleFor(x => x.TargetResolutionHours).InclusiveBetween(1, 8760)
            .WithMessage("Target resolution must be between 1 and 8760 hours (1 year).");
        RuleFor(x => x.CreatedBy).GreaterThan(0).WithMessage("CreatedBy must be a valid user ID.");
    }
}
