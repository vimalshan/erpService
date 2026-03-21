using FluentValidation;

namespace TravelService.Application.TourPlans.Commands.CreateTourPlan;

public class CreateTourPlanValidator : AbstractValidator<CreateTourPlanCommand>
{
    public CreateTourPlanValidator()
    {
        RuleFor(x => x.EmployeeSysId).NotEmpty().MaximumLength(255);
        RuleFor(x => x.Purpose).NotEmpty().MaximumLength(255);
        RuleFor(x => x.Remarks).NotEmpty().MaximumLength(255);
        RuleFor(x => x.Category).NotEmpty().Must(c => c == "Domestic" || c == "International")
            .WithMessage("Category must be 'Domestic' or 'International'.");
        RuleFor(x => x.FromCityId).NotEmpty();
        RuleFor(x => x.FromCityName).NotEmpty();
        RuleFor(x => x.ToCityId).NotEmpty();
        RuleFor(x => x.ToCityName).NotEmpty();
        RuleFor(x => x.SupervisorRemarks).NotEmpty().MaximumLength(255);
        RuleFor(x => x.PayrollUnitId).NotEmpty();
        RuleFor(x => x.StartDate).GreaterThanOrEqualTo(DateTime.Today.AddDays(-1));
        RuleFor(x => x.EndDate).GreaterThanOrEqualTo(x => x.StartDate).When(x => x.EndDate.HasValue);
    }
}
