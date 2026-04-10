using FluentValidation;
using SparshTransactional.Application.Commands;

namespace SparshTransactional.Application.Validators;

public class CreateScholarshipValidator : AbstractValidator<CreateScholarshipCommand>
{
    public CreateScholarshipValidator()
    {
        RuleFor(x => x.Name).NotEmpty().MaximumLength(200);
        RuleFor(x => x.CreatedBy).GreaterThan(0);
        RuleFor(x => x.CoveragePercent).InclusiveBetween(0, 100).When(x => x.CoveragePercent.HasValue);
        RuleFor(x => x.MaxAmount).GreaterThan(0).When(x => x.MaxAmount.HasValue);
    }
}

public class SubmitApplicationValidator : AbstractValidator<SubmitApplicationCommand>
{
    public SubmitApplicationValidator()
    {
        RuleFor(x => x.StudentId).GreaterThan(0);
        RuleFor(x => x.ScholarshipId).GreaterThan(0);
        RuleFor(x => x.CreatedBy).GreaterThan(0);
        RuleFor(x => x.FamilyIncome).GreaterThanOrEqualTo(0).When(x => x.FamilyIncome.HasValue);
    }
}

public class ApproveApplicationValidator : AbstractValidator<ApproveApplicationCommand>
{
    public ApproveApplicationValidator()
    {
        RuleFor(x => x.ApplicationId).GreaterThan(0);
        RuleFor(x => x.ApprovedBy).GreaterThan(0);
        RuleFor(x => x.ApprovedAmount).GreaterThan(0);
    }
}

public class RejectApplicationValidator : AbstractValidator<RejectApplicationCommand>
{
    public RejectApplicationValidator()
    {
        RuleFor(x => x.ApplicationId).GreaterThan(0);
        RuleFor(x => x.RejectedBy).GreaterThan(0);
    }
}

public class CreateDisbursementValidator : AbstractValidator<CreateDisbursementCommand>
{
    public CreateDisbursementValidator()
    {
        RuleFor(x => x.ApplicationId).GreaterThan(0);
        RuleFor(x => x.Amount).GreaterThan(0);
        RuleFor(x => x.CreatedBy).GreaterThan(0);
    }
}

public class CompleteDisbursementValidator : AbstractValidator<CompleteDisbursementCommand>
{
    public CompleteDisbursementValidator()
    {
        RuleFor(x => x.DisbursementId).GreaterThan(0);
        RuleFor(x => x.PaymentReference).NotEmpty().MaximumLength(100);
    }
}

public class AddEligibilityCriteriaValidator : AbstractValidator<AddEligibilityCriteriaCommand>
{
    public AddEligibilityCriteriaValidator()
    {
        RuleFor(x => x.ScholarshipId).GreaterThan(0);
        RuleFor(x => x.CriteriaName).NotEmpty().MaximumLength(200);
        RuleFor(x => x.CreatedBy).GreaterThan(0);
    }
}
