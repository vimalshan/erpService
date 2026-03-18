using FluentValidation;

namespace UtilityService.Application.Commands.CreateToadPlanSql;

public class CreateToadPlanSqlCommandValidator : AbstractValidator<CreateToadPlanSqlCommand>
{
    public CreateToadPlanSqlCommandValidator()
    {
        RuleFor(x => x.StatementId)
            .NotEmpty().WithMessage("StatementId is required.")
            .MaximumLength(32).WithMessage("StatementId cannot exceed 32 characters.");

        RuleFor(x => x.Username)
            .MaximumLength(30).WithMessage("Username cannot exceed 30 characters.")
            .When(x => x.Username is not null);

        RuleFor(x => x.Statement)
            .MaximumLength(2000).WithMessage("Statement cannot exceed 2000 characters.")
            .When(x => x.Statement is not null);
    }
}
