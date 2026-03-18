using FluentValidation;
using PromotionService.Features.Commands;

namespace PromotionService.Validators;

public class CreateRatingCommandValidator : AbstractValidator<CreateRatingCommand>
{
    public CreateRatingCommandValidator()
    {
        RuleFor(x => x.EmployeeSystemId).GreaterThan(0).WithMessage("Valid EmployeeSystemId required.");
        RuleFor(x => x.DDYear).InclusiveBetween(2000, 2100).WithMessage("DDYear must be a valid fiscal year.");
        RuleFor(x => x.AppraisalScore).InclusiveBetween(0, 5).WithMessage("Appraisal score 0–5.");
        RuleFor(x => x.CompetencyScore).InclusiveBetween(0, 5).WithMessage("Competency score 0–5.");
        RuleFor(x => x.GoalCompletionScore).InclusiveBetween(0, 5).WithMessage("Goal completion score 0–5.");
    }
}

public class UpdateRatingCommandValidator : AbstractValidator<UpdateRatingCommand>
{
    public UpdateRatingCommandValidator()
    {
        RuleFor(x => x.RatingId).GreaterThan(0);
        RuleFor(x => x.AppraisalScore).InclusiveBetween(0, 5);
        RuleFor(x => x.CompetencyScore).InclusiveBetween(0, 5);
        RuleFor(x => x.GoalCompletionScore).InclusiveBetween(0, 5);
    }
}

public class CreatePromotionRecommendationCommandValidator : AbstractValidator<CreatePromotionRecommendationCommand>
{
    public CreatePromotionRecommendationCommandValidator()
    {
        RuleFor(x => x.EmployeeSystemId).GreaterThan(0);
        RuleFor(x => x.RatingId).GreaterThan(0);
        RuleFor(x => x.CurrentDesignation).NotEmpty().MaximumLength(200);
        RuleFor(x => x.CurrentGrade).NotEmpty().MaximumLength(50);
        RuleFor(x => x.ProposedDesignation).NotEmpty().MaximumLength(200);
        RuleFor(x => x.ProposedGrade).NotEmpty().MaximumLength(50);
        RuleFor(x => x.PromotionEffectiveDate).GreaterThan(DateTime.UtcNow.AddDays(-1))
            .WithMessage("Effective date must be today or future.");
        RuleFor(x => x.ProposedSalaryIncrease).GreaterThanOrEqualTo(0);
    }
}

public class CreateIncrementRequestCommandValidator : AbstractValidator<CreateIncrementRequestCommand>
{
    public CreateIncrementRequestCommandValidator()
    {
        RuleFor(x => x.EmployeeSystemId).GreaterThan(0);
        RuleFor(x => x.RatingId).GreaterThan(0);
        RuleFor(x => x.IncrementType).NotEmpty().Must(t => new[] { "Annual", "Special", "Merit" }.Contains(t))
            .WithMessage("IncrementType must be Annual, Special, or Merit.");
        RuleFor(x => x.CurrentBaseSalary).GreaterThan(0);
        RuleFor(x => x.ProposedBaseSalary).GreaterThan(x => x.CurrentBaseSalary)
            .WithMessage("Proposed salary must be greater than current.");
        RuleFor(x => x.IncrementReason).NotEmpty().MaximumLength(500);
        RuleFor(x => x.EffectiveFromDate).GreaterThan(DateTime.UtcNow.AddDays(-1));
    }
}

public class CreateVTCAssessmentCommandValidator : AbstractValidator<CreateVTCAssessmentCommand>
{
    public CreateVTCAssessmentCommandValidator()
    {
        RuleFor(x => x.EmployeeSystemId).GreaterThan(0);
        RuleFor(x => x.DDYear).InclusiveBetween(2000, 2100);
        RuleFor(x => x.Quarter).InclusiveBetween(1, 4).WithMessage("Quarter must be 1–4.");
        RuleFor(x => x.Score).InclusiveBetween(0, 100);
    }
}
