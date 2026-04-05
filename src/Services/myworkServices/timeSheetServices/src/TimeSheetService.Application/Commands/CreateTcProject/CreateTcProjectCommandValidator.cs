using FluentValidation;

namespace TimeSheetService.Application.Commands.CreateTcProject;

public class CreateTcProjectCommandValidator : AbstractValidator<CreateTcProjectCommand>
{
    public CreateTcProjectCommandValidator()
    {
        RuleFor(x => x.ProjectName).NotEmpty().MaximumLength(200);
        RuleFor(x => x.CategoryId).GreaterThan(0);
        RuleFor(x => x.TeamId).GreaterThan(0);
        RuleFor(x => x.EffectiveDate).NotEmpty();
        RuleFor(x => x.ModifiedBy).GreaterThan(0);
    }
}
