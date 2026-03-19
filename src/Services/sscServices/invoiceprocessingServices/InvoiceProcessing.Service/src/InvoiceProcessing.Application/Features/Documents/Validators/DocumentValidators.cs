using FluentValidation;
using InvoiceProcessing.Application.Features.Documents.Commands;

namespace InvoiceProcessing.Application.Features.Documents.Validators;

public class CreateDocumentCommandValidator : AbstractValidator<CreateDocumentCommand>
{
    public CreateDocumentCommandValidator()
    {
        RuleFor(x => x.OrgId).NotEmpty().MaximumLength(25);
        RuleFor(x => x.DocumentType).NotEmpty().MaximumLength(3);
        RuleFor(x => x.InvoiceNo).NotEmpty().MaximumLength(100);
        RuleFor(x => x.InvoiceAmount).GreaterThanOrEqualTo(0);
        RuleFor(x => x.InvoiceDate).NotEmpty().LessThanOrEqualTo(DateTime.UtcNow.AddDays(1));
        RuleFor(x => x.InvoiceReceiptDate).NotEmpty();
        RuleFor(x => x.PaymentDueDate).NotEmpty();
        RuleFor(x => x.Pages).GreaterThan(0);
        RuleFor(x => x.AccountCode).NotEmpty().MaximumLength(25);
        RuleFor(x => x.PoNumber).NotEmpty().MaximumLength(25);
    }
}

public class ApproveDocumentCommandValidator : AbstractValidator<ApproveDocumentCommand>
{
    public ApproveDocumentCommandValidator()
    {
        RuleFor(x => x.Id).GreaterThan(0);
        RuleFor(x => x.ApprovedBy).GreaterThan(0);
    }
}

public class CancelDocumentCommandValidator : AbstractValidator<CancelDocumentCommand>
{
    public CancelDocumentCommandValidator()
    {
        RuleFor(x => x.Id).GreaterThan(0);
        RuleFor(x => x.CancelledBy).GreaterThan(0);
    }
}
