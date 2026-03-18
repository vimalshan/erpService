using FluentValidation;

namespace TdsService.Application.Vendors.Commands.UpdateTdsVendor;

public sealed class UpdateTdsVendorCommandValidator : AbstractValidator<UpdateTdsVendorCommand>
{
    public UpdateTdsVendorCommandValidator()
    {
        RuleFor(x => x.VendorId)
            .GreaterThan(0).WithMessage("Vendor ID must be a positive number.");

        RuleFor(x => x.VendorName)
            .NotEmpty().WithMessage("Vendor name is required.")
            .MaximumLength(240);

        RuleFor(x => x.EmailAddress)
            .EmailAddress()
            .When(x => !string.IsNullOrWhiteSpace(x.EmailAddress));

        RuleFor(x => x.PanNo)
            .Matches(@"^[A-Z]{5}[0-9]{4}[A-Z]$")
            .When(x => !string.IsNullOrWhiteSpace(x.PanNo));
    }
}
