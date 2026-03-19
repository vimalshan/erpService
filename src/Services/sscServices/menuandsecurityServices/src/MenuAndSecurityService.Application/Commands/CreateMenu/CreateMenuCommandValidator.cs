using FluentValidation;

namespace MenuAndSecurityService.Application.Commands.CreateMenu;

public class CreateMenuCommandValidator : AbstractValidator<CreateMenuCommand>
{
    public CreateMenuCommandValidator()
    {
        RuleFor(x => x.MenuId).GreaterThan(0);
        RuleFor(x => x.MenuName).NotEmpty().MaximumLength(100);
        RuleFor(x => x.MenuPageName).NotEmpty().MaximumLength(200);
        RuleFor(x => x.MenuDisplayOrder).GreaterThan(0);
        RuleFor(x => x.ModifiedBy).GreaterThan(0);
    }
}
