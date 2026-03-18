using FluentValidation;

namespace TdsService.Application.Vendors.Commands.CreateTdsVendor;

public sealed class CreateTdsVendorCommandValidator : AbstractValidator<CreateTdsVendorCommand>
{
    public CreateTdsVendorCommandValidator()
    {
        RuleFor(x => x.VendorId)
            .GreaterThan(0).WithMessage("Vendor ID must be a positive number.");

        RuleFor(x => x.VendorName)
            .NotEmpty().WithMessage("Vendor name is required.")
            .MaximumLength(240).WithMessage("Vendor name must not exceed 240 characters.");

        RuleFor(x => x.EmailAddress)
            .EmailAddress().WithMessage("A valid email address is required.")
            .MaximumLength(3000)
            .When(x => !string.IsNullOrWhiteSpace(x.EmailAddress));

        RuleFor(x => x.PanNo)
            .Matches(@"^[A-Z]{5}[0-9]{4}[A-Z]$")
            .WithMessage("PAN number must be in the format AAAAA0000A.")
            .When(x => !string.IsNullOrWhiteSpace(x.PanNo));
    }
}
