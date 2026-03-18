using FluentValidation;
using ProjectService.Application.Commands;

namespace ProjectService.Application.Validators;

public class CreateProjectCommandValidator : AbstractValidator<CreateProjectCommand>
{
    public CreateProjectCommandValidator()
    {
        RuleFor(x => x.ProjName).NotEmpty().MaximumLength(50);
        RuleFor(x => x.ProjCharterNo).GreaterThanOrEqualTo(0);
        RuleFor(x => x.ProjLeaderId).GreaterThan(0);
        RuleFor(x => x.ProjTypeId).GreaterThan(0);
        RuleFor(x => x.ProjLocId).GreaterThan(0);
        RuleFor(x => x.ProjProcessId).GreaterThan(0);
        RuleFor(x => x.ProjStartDate).NotEmpty();
        RuleFor(x => x.ProjEndDate).GreaterThan(x => x.ProjStartDate).WithMessage("End date must be after start date.");
        RuleFor(x => x.ProjEstEndDate).GreaterThan(x => x.ProjStartDate).WithMessage("Estimated end date must be after start date.");
    }
}

public class UpdateProjectCommandValidator : AbstractValidator<UpdateProjectCommand>
{
    public UpdateProjectCommandValidator()
    {
        RuleFor(x => x.ProjId).GreaterThan(0);
        RuleFor(x => x.ProjName).NotEmpty().MaximumLength(50);
        RuleFor(x => x.ProjLeaderId).GreaterThan(0);
        RuleFor(x => x.ProjEndDate).GreaterThan(x => x.ProjStartDate);
    }
}

public class HoldProjectCommandValidator : AbstractValidator<HoldProjectCommand>
{
    public HoldProjectCommandValidator()
    {
        RuleFor(x => x.ProjId).GreaterThan(0);
        RuleFor(x => x.Reason).NotEmpty().MaximumLength(150);
        RuleFor(x => x.UpdatedBy).GreaterThan(0);
    }
}

public class AddProjectMemberCommandValidator : AbstractValidator<AddProjectMemberCommand>
{
    public AddProjectMemberCommandValidator()
    {
        RuleFor(x => x.ProjId).GreaterThan(0);
        RuleFor(x => x.FuncId).GreaterThan(0);
        RuleFor(x => x.EmpSysId).GreaterThan(0);
    }
}
