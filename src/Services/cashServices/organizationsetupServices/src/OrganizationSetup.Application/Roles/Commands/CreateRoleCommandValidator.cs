using FluentValidation;

namespace OrganizationSetup.Application.Roles.Commands;

public class CreateRoleCommandValidator : AbstractValidator<CreateRoleCommand>
{
    public CreateRoleCommandValidator()
    {
        RuleFor(x => x.RoleId).GreaterThan(0).WithMessage("RoleId must be greater than 0");
        RuleFor(x => x.RoleName).NotEmpty().MinimumLength(2).MaximumLength(50);
        RuleFor(x => x.RoleLevel).GreaterThan(0).WithMessage("RoleLevel must be greater than 0");
    }
}
