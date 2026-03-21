using FluentValidation;
using SecurityService.Application.Commands;

namespace SecurityService.Application.Validators;

public class CreateUserCommandValidator : AbstractValidator<CreateUserCommand>
{
    public CreateUserCommandValidator()
    {
        RuleFor(x => x.Dto.Username).NotEmpty().MaximumLength(50);
        RuleFor(x => x.Dto.Password).NotEmpty().MinimumLength(8);
        RuleFor(x => x.Dto.Email).NotEmpty().EmailAddress().MaximumLength(100);
        RuleFor(x => x.Dto.FullName).NotEmpty().MaximumLength(100);
    }
}

public class LoginCommandValidator : AbstractValidator<LoginCommand>
{
    public LoginCommandValidator()
    {
        RuleFor(x => x.Dto.Username).NotEmpty();
        RuleFor(x => x.Dto.Password).NotEmpty();
    }
}

public class CreateRoleCommandValidator : AbstractValidator<CreateRoleCommand>
{
    public CreateRoleCommandValidator()
    {
        RuleFor(x => x.Dto.RoleName).NotEmpty().MaximumLength(50);
    }
}

public class CreatePermissionCommandValidator : AbstractValidator<CreatePermissionCommand>
{
    public CreatePermissionCommandValidator()
    {
        RuleFor(x => x.Dto.PermissionName).NotEmpty().MaximumLength(100);
    }
}
