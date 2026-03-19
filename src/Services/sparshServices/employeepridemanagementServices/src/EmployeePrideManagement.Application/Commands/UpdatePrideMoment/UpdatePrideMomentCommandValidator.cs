using FluentValidation;

namespace EmployeePrideManagement.Application.Commands.UpdatePrideMoment;

public class UpdatePrideMomentCommandValidator : AbstractValidator<UpdatePrideMomentCommand>
{
    public UpdatePrideMomentCommandValidator()
    {
        RuleFor(x => x.MomentPrideId)
            .GreaterThan(0).WithMessage("Pride Moment ID must be greater than zero.");

        RuleFor(x => x.Title)
            .NotEmpty().WithMessage("Title is required.")
            .MaximumLength(50).WithMessage("Title must not exceed 50 characters.");

        RuleFor(x => x.Footer)
            .NotEmpty().WithMessage("Footer is required.")
            .MaximumLength(500).WithMessage("Footer must not exceed 500 characters.");

        RuleFor(x => x.Location)
            .NotEmpty().WithMessage("Location is required.")
            .MaximumLength(100).WithMessage("Location must not exceed 100 characters.");

        RuleFor(x => x.ImagePath)
            .NotEmpty().WithMessage("Image path is required.")
            .MaximumLength(200).WithMessage("Image path must not exceed 200 characters.");

        RuleFor(x => x.ModifiedBy)
            .GreaterThan(0).WithMessage("ModifiedBy must be greater than zero.");
    }
}
