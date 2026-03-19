using FluentValidation;
using HRDocumentService.Application.Commands;

namespace HRDocumentService.Application.Validators;

public sealed class CreateHRDocumentValidator : AbstractValidator<CreateHRDocumentCommand>
{
    public CreateHRDocumentValidator()
    {
        RuleFor(x => x.DocType).NotEmpty().Length(3).WithMessage("DocType must be 3 characters.");
        RuleFor(x => x.DocPayRefNo).GreaterThan(0);
        RuleFor(x => x.DocLocId).GreaterThan(0);
        RuleFor(x => x.DocUnitId).GreaterThan(0);
        RuleFor(x => x.DocRemarks).NotEmpty().MaximumLength(100);
        RuleFor(x => x.DocUserId).GreaterThan(0);
        RuleFor(x => x.DocSource).NotEmpty().Length(3).WithMessage("DocSource must be 3 characters.");
        RuleFor(x => x.DocRefNo).MaximumLength(50).When(x => x.DocRefNo is not null);
        RuleFor(x => x.DocRefName).MaximumLength(200).When(x => x.DocRefName is not null);
    }
}

public sealed class ApproveHRDocumentValidator : AbstractValidator<ApproveHRDocumentCommand>
{
    public ApproveHRDocumentValidator()
    {
        RuleFor(x => x.DocId).GreaterThan(0);
        RuleFor(x => x.ApprovedBy).GreaterThan(0);
    }
}

public sealed class RejectHRDocumentValidator : AbstractValidator<RejectHRDocumentCommand>
{
    public RejectHRDocumentValidator()
    {
        RuleFor(x => x.DocId).GreaterThan(0);
        RuleFor(x => x.RejectedBy).GreaterThan(0);
        RuleFor(x => x.RejectRemarks).NotEmpty().MaximumLength(200);
    }
}
