using FluentValidation;
using BatchAndEnvelopeService.Application.Commands.Envelope;

namespace BatchAndEnvelopeService.Application.Validators;

public class CreateEnvelopeCommandValidator : AbstractValidator<CreateEnvelopeCommand>
{
    public CreateEnvelopeCommandValidator()
    {
        RuleFor(x => x.EnvelopeType).NotEmpty().MaximumLength(3).WithMessage("EnvelopeType is required, max 3 chars.");
        RuleFor(x => x.CreatedBy).GreaterThan(0).WithMessage("CreatedBy must be a valid user ID.");
        RuleFor(x => x.LocationId).GreaterThan(0).WithMessage("LocationId must be valid.");
        RuleFor(x => x.Documents).NotEmpty().WithMessage("At least one document is required.");
    }
}
