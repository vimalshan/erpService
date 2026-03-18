using FluentValidation;
using RecruitmentService.Application.Commands.Vacancies;

namespace RecruitmentService.Application.Validators;

public class CreateVacancyCommandValidator : AbstractValidator<CreateVacancyCommand>
{
    public CreateVacancyCommandValidator()
    {
        RuleFor(x => x.Request.VacancyId).GreaterThan(0);
        RuleFor(x => x.Request.VacancyUnit).NotEmpty().MaximumLength(3);
        RuleFor(x => x.Request.VacancyName).NotEmpty().MaximumLength(65);
        RuleFor(x => x.Request.VacancyAge).NotEmpty().MaximumLength(65);
        RuleFor(x => x.Request.VacancyExperience).NotEmpty().MaximumLength(4000);
        RuleFor(x => x.Request.VacancyQualification).NotEmpty().MaximumLength(4000);
        RuleFor(x => x.Request.VacancyGrade).GreaterThan(0);
        RuleFor(x => x.Request.VacancyPositionId).GreaterThan(0);
        RuleFor(x => x.Request.VacancyLocation).GreaterThan(0);
        RuleFor(x => x.Request.VacancyProcess).GreaterThan(0);
        RuleFor(x => x.Request.VacancyUnitId).GreaterThan(0);
        RuleFor(x => x.PostedBy).GreaterThan(0);

        When(x => x.Request.VacancyLastDate.HasValue, () =>
            RuleFor(x => x.Request.VacancyLastDate!.Value).GreaterThan(DateTime.UtcNow)
                .WithMessage("Last date must be in the future."));

        When(x => x.Request.CtcFrom.HasValue && x.Request.CtcTo.HasValue, () =>
            RuleFor(x => x.Request.CtcTo!.Value)
                .GreaterThanOrEqualTo(x => x.Request.CtcFrom!.Value)
                .WithMessage("CTC To must be >= CTC From."));
    }
}
