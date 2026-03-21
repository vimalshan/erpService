using FluentValidation;

namespace TourPlanService.Application.Commands.CreateTourPlan;

public sealed class CreateTourPlanCommandValidator : AbstractValidator<CreateTourPlanCommand>
{
    public CreateTourPlanCommandValidator()
    {
        RuleFor(x => x.TpId).NotEmpty().MaximumLength(255);
        RuleFor(x => x.TpEmpSysId).NotEmpty().MaximumLength(255);
        RuleFor(x => x.TpStartDate).NotEmpty().GreaterThan(DateTime.UtcNow.AddYears(-1));
        RuleFor(x => x.TpPurpose).NotEmpty().MaximumLength(255);
        RuleFor(x => x.TpRemarks).NotEmpty().MaximumLength(255);
        RuleFor(x => x.TpCategory).NotEmpty().Must(c => c == "DOM" || c == "INT")
            .WithMessage("TpCategory must be 'DOM' or 'INT'.");
        RuleFor(x => x.TpBookInc).NotEmpty().MaximumLength(255);
        RuleFor(x => x.TpFromCityId).NotEmpty().MaximumLength(255);
        RuleFor(x => x.TpFromCityName).NotEmpty().MaximumLength(255);
        RuleFor(x => x.TpToCityId).NotEmpty().MaximumLength(255);
        RuleFor(x => x.TpToCityName).NotEmpty().MaximumLength(255);
        RuleFor(x => x.TpSupRemarks).NotEmpty().MaximumLength(255);
        RuleFor(x => x.CreatedBy).NotEmpty().MaximumLength(255);
        RuleFor(x => x.TpEndDate)
            .GreaterThan(x => x.TpStartDate).When(x => x.TpEndDate.HasValue)
            .WithMessage("End date must be after start date.");
    }
}
