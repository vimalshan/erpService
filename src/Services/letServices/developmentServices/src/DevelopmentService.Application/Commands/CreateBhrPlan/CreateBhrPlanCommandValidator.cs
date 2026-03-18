using FluentValidation;

namespace DevelopmentService.Application.Commands.CreateBhrPlan;

public class CreateBhrPlanCommandValidator : AbstractValidator<CreateBhrPlanCommand>
{
    public CreateBhrPlanCommandValidator()
    {
        RuleFor(x => x.ReqNum).GreaterThan(0);
        RuleFor(x => x.UserId).NotEmpty().MaximumLength(255);
        RuleFor(x => x.TrainingProgram).NotEmpty().MaximumLength(255);
        RuleFor(x => x.TrainingCode).GreaterThan(0);
        RuleFor(x => x.Priority).InclusiveBetween(1, 100);
        RuleFor(x => x.BhrAccept).Must(s => s is 'A' or 'R').WithMessage("BhrAccept must be A or R.");
    }
}
