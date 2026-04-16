using ActionService.Application.Commands;
using FluentValidation;

namespace ActionService.Validators;

public class CreateActionCommandValidator : AbstractValidator<CreateActionCommand>
{
    public CreateActionCommandValidator()
    {
        RuleFor(x => x.Dto.Action).NotEmpty().MaximumLength(255);
        RuleFor(x => x.Dto.Language).MaximumLength(50);
        RuleFor(x => x.Dto.Service).MaximumLength(100);
        RuleFor(x => x.Dto.Site).MaximumLength(100);
        RuleFor(x => x.Dto.EntityType).MaximumLength(100);
        RuleFor(x => x.Dto.Subject).MaximumLength(255);
        RuleFor(x => x.Dto.SnowLink).MaximumLength(255);
    }
}
