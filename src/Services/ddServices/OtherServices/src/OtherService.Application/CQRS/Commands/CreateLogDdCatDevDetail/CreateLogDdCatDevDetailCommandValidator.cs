using FluentValidation;

namespace OtherService.Application.CQRS.Commands.CreateLogDdCatDevDetail;

public sealed class CreateLogDdCatDevDetailCommandValidator
    : AbstractValidator<CreateLogDdCatDevDetailCommand>
{
    public CreateLogDdCatDevDetailCommandValidator()
    {
        RuleFor(x => x.AppId)
            .NotEmpty().WithMessage("AppId is required.")
            .MaximumLength(30).WithMessage("AppId cannot exceed 30 characters.");

        RuleFor(x => x.AppNum)
            .GreaterThanOrEqualTo(0).WithMessage("AppNum must be non-negative.");

        RuleFor(x => x.Desc)
            .MaximumLength(400).WithMessage("Desc cannot exceed 400 characters.")
            .When(x => x.Desc is not null);

        RuleFor(x => x.Need)
            .MaximumLength(400).WithMessage("Need cannot exceed 400 characters.")
            .When(x => x.Need is not null);
    }
}
