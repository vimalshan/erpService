using FluentValidation;
using LocationServices.Application.Commands;

namespace LocationServices.Application.Validators;

public sealed class CreateLocationAppMapCommandValidator
    : AbstractValidator<CreateLocationAppMapCommand>
{
    public CreateLocationAppMapCommandValidator()
    {
        RuleFor(x => x.LocationId)
            .GreaterThan(0).WithMessage("LocationId must be greater than zero.");

        RuleFor(x => x.AppName)
            .NotEmpty().WithMessage("AppName is required.")
            .MaximumLength(255).WithMessage("AppName cannot exceed 255 characters.");

        RuleFor(x => x.CreatedBy)
            .NotEmpty().WithMessage("CreatedBy is required.")
            .MaximumLength(100);

        RuleFor(x => x.DeemedApproval)
            .Must(v => v is null || v == "Y" || v == "N")
            .WithMessage("DeemedApproval must be 'Y', 'N', or null.");

        RuleFor(x => x.SelfAccess)
            .MaximumLength(255).When(x => x.SelfAccess is not null);
    }
}

public sealed class UpdateLocationAppMapCommandValidator
    : AbstractValidator<UpdateLocationAppMapCommand>
{
    public UpdateLocationAppMapCommandValidator()
    {
        RuleFor(x => x.LocationId).GreaterThan(0);
        RuleFor(x => x.AppName).NotEmpty().MaximumLength(255);
        RuleFor(x => x.ModifiedBy).NotEmpty().MaximumLength(100);
        RuleFor(x => x.DeemedApproval)
            .Must(v => v is null || v == "Y" || v == "N")
            .WithMessage("DeemedApproval must be 'Y', 'N', or null.");
    }
}
