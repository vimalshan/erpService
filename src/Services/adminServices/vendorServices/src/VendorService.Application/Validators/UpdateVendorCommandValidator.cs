using FluentValidation;
using VendorService.Application.Commands;

namespace VendorService.Application.Validators;

public sealed class UpdateVendorCommandValidator : AbstractValidator<UpdateVendorCommand>
{
    public UpdateVendorCommandValidator()
    {
        RuleFor(x => x.VendorId).GreaterThan(0).WithMessage("Vendor ID must be positive.");
        RuleFor(x => x.CategoryId).GreaterThan(0).WithMessage("Category ID must be positive.");
        RuleFor(x => x.LocationId).GreaterThan(0).WithMessage("Location ID must be positive.");
        RuleFor(x => x.Name).NotEmpty().MaximumLength(100);
        RuleFor(x => x.Email).MaximumLength(50).EmailAddress().When(x => !string.IsNullOrWhiteSpace(x.Email));
        RuleFor(x => x.Address).NotEmpty().MaximumLength(200);
        RuleFor(x => x.UpdatedBy).GreaterThan(0).WithMessage("UpdatedBy must be a valid user ID.");
        RuleFor(x => x.LiveStatus).Must(s => s == "A" || s == "I").WithMessage("LiveStatus must be 'A' or 'I'.");
    }
}
