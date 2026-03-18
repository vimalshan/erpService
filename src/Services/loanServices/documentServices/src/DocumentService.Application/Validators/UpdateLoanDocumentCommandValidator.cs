using FluentValidation;
using DocumentService.Application.Commands.UpdateLoanDocument;

namespace DocumentService.Application.Validators;

public class UpdateLoanDocumentCommandValidator : AbstractValidator<UpdateLoanDocumentCommand>
{
    public UpdateLoanDocumentCommandValidator()
    {
        RuleFor(x => x.Id).GreaterThan(0).WithMessage("Document ID must be a positive number.");
        RuleFor(x => x.TypeId).GreaterThan(0).WithMessage("Type ID must be a positive number.");
        RuleFor(x => x.ModifiedBy).GreaterThan(0).WithMessage("Modified By must be a positive number.");
    }
}
