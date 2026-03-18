using FluentValidation;
using DocumentService.Application.Commands.CreateLoanDocument;

namespace DocumentService.Application.Validators;

public class CreateLoanDocumentCommandValidator : AbstractValidator<CreateLoanDocumentCommand>
{
    public CreateLoanDocumentCommandValidator()
    {
        RuleFor(x => x.Id).GreaterThan(0).WithMessage("Document ID must be a positive number.");
        RuleFor(x => x.LoanId).GreaterThan(0).WithMessage("Loan ID must be a positive number.");
        RuleFor(x => x.TypeId).GreaterThan(0).WithMessage("Type ID must be a positive number.");
        RuleFor(x => x.ModifiedBy).GreaterThan(0).WithMessage("Modified By must be a positive number.");
    }
}
