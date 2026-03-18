using FluentValidation;

namespace DevelopmentService.Application.Commands.CreateLearningPlan;

public class CreateLearningPlanCommandValidator : AbstractValidator<CreateLearningPlanCommand>
{
    public CreateLearningPlanCommandValidator()
    {
        RuleFor(x => x.ReqNum).GreaterThan(0).WithMessage("ReqNum must be positive.");
        RuleFor(x => x.UserId).NotEmpty().MaximumLength(255).WithMessage("UserId is required.");
        RuleFor(x => x.PinNum).GreaterThan(0).WithMessage("PinNum must be positive.");
        RuleFor(x => x.DevSource).NotEmpty().MaximumLength(255);
        RuleFor(x => x.DevNeed).NotEmpty().MaximumLength(255);
        RuleFor(x => x.Priority).InclusiveBetween(1, 100);
        RuleFor(x => x.EntDate).LessThanOrEqualTo(DateTime.UtcNow.AddMinutes(5));
    }
}
