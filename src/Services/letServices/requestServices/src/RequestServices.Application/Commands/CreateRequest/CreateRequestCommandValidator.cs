using FluentValidation;

namespace RequestServices.Application.Commands.CreateRequest;

public class CreateRequestCommandValidator : AbstractValidator<CreateRequestCommand>
{
    public CreateRequestCommandValidator()
    {
        RuleFor(x => x.RequestId)    .GreaterThan(0);
        RuleFor(x => x.EmployeeUser) .NotEmpty().MaximumLength(25);
        RuleFor(x => x.SupervisorUser).NotEmpty().MaximumLength(25);
        RuleFor(x => x.TrainingNeed) .NotEmpty().MaximumLength(255);
        RuleFor(x => x.CourseId)     .GreaterThan(0);
        RuleFor(x => x.StartDate)    .LessThan(x => x.EndDate).WithMessage("Start date must be before end date.");
        RuleFor(x => x.BusinessBenefit)    .NotEmpty().MaximumLength(255);
        RuleFor(x => x.ExpectedCompetency) .NotEmpty().MaximumLength(255);
        RuleFor(x => x.CourseDescription)  .NotEmpty().MaximumLength(255);
    }
}
