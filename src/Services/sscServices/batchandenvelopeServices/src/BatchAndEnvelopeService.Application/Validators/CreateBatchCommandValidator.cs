using FluentValidation;
using BatchAndEnvelopeService.Application.Commands.Batch;

namespace BatchAndEnvelopeService.Application.Validators;

public class CreateBatchCommandValidator : AbstractValidator<CreateBatchCommand>
{
    public CreateBatchCommandValidator()
    {
        RuleFor(x => x.CreatedBy).GreaterThan(0).WithMessage("CreatedBy must be a valid user ID.");
        RuleFor(x => x.LocationId).GreaterThan(0).WithMessage("LocationId must be valid.");
        RuleFor(x => x.ReceivedBy).GreaterThan(0).WithMessage("ReceivedBy must be valid.");
        RuleFor(x => x.PodNo).NotEmpty().MaximumLength(25).WithMessage("PodNo is required and max 25 characters.");
        RuleFor(x => x.CourierName).MaximumLength(100).When(x => x.CourierName != null);
        RuleFor(x => x.EnvelopeIds).NotEmpty().WithMessage("At least one envelope is required.");
    }
}
