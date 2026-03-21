using FluentValidation;
using TravelRequestService.Application.Commands;

namespace TravelRequestService.Application.Validators;

public class CreateTravelRequestCommandValidator : AbstractValidator<CreateTravelRequestCommand>
{
    public CreateTravelRequestCommandValidator()
    {
        RuleFor(x => x.CompanyCode)
            .NotEmpty().WithMessage("Company code is required.")
            .MaximumLength(3).WithMessage("Company code must not exceed 3 characters.");

        RuleFor(x => x.UserNumber)
            .GreaterThan(0).WithMessage("User number must be greater than 0.");

        RuleFor(x => x.TravelType)
            .NotEmpty().WithMessage("Travel type is required.")
            .Must(x => x is "Domestic" or "International")
            .WithMessage("Travel type must be 'Domestic' or 'International'.");

        RuleFor(x => x.BudgetAmount)
            .GreaterThanOrEqualTo(0).When(x => x.BudgetAmount.HasValue)
            .WithMessage("Budget amount cannot be negative.");
    }
}

public class ApproveTravelRequestCommandValidator : AbstractValidator<ApproveTravelRequestCommand>
{
    public ApproveTravelRequestCommandValidator()
    {
        RuleFor(x => x.PlanNumber).GreaterThan(0);
        RuleFor(x => x.ApprovedBy).GreaterThan(0);
        RuleFor(x => x.ApprovalAmount).GreaterThanOrEqualTo(0);
    }
}

public class AddTravelAdvanceCommandValidator : AbstractValidator<AddTravelAdvanceCommand>
{
    public AddTravelAdvanceCommandValidator()
    {
        RuleFor(x => x.RequestNumber).GreaterThan(0);
        RuleFor(x => x.AdvanceAmount).GreaterThan(0);
    }
}
