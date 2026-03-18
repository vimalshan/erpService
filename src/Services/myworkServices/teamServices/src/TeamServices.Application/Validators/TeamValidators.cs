using FluentValidation;
using TeamServices.Application.Commands;

namespace TeamServices.Application.Validators;

public class CreateTeamCommandValidator : AbstractValidator<CreateTeamCommand>
{
    public CreateTeamCommandValidator()
    {
        RuleFor(x => x.TeamId).GreaterThan(0);
        RuleFor(x => x.TeamName).NotEmpty().MaximumLength(50);
        RuleFor(x => x.ModifiedBy).GreaterThan(0);
    }
}

public class UpdateTeamCommandValidator : AbstractValidator<UpdateTeamCommand>
{
    public UpdateTeamCommandValidator()
    {
        RuleFor(x => x.TeamId).GreaterThan(0);
        RuleFor(x => x.TeamName).NotEmpty().MaximumLength(50);
        RuleFor(x => x.ModifiedBy).GreaterThan(0);
    }
}

public class AddTeamEmployeeCommandValidator : AbstractValidator<AddTeamEmployeeCommand>
{
    public AddTeamEmployeeCommandValidator()
    {
        RuleFor(x => x.Id).GreaterThan(0);
        RuleFor(x => x.TeamId).GreaterThan(0);
        RuleFor(x => x.EmployeeSysId).GreaterThan(0);
        RuleFor(x => x.EffectiveDate).NotEmpty();
        RuleFor(x => x.CloseDate)
            .GreaterThan(x => x.EffectiveDate)
            .When(x => x.CloseDate.HasValue);
        RuleFor(x => x.ModifiedBy).GreaterThan(0);
    }
}

public class AddTeamUnitMapCommandValidator : AbstractValidator<AddTeamUnitMapCommand>
{
    public AddTeamUnitMapCommandValidator()
    {
        RuleFor(x => x.MapId).GreaterThan(0);
        RuleFor(x => x.TeamId).GreaterThan(0);
        RuleFor(x => x.UnitId).GreaterThan(0);
        RuleFor(x => x.GradeCategory).NotEmpty();
        RuleFor(x => x.ModifiedBy).GreaterThan(0);
    }
}
