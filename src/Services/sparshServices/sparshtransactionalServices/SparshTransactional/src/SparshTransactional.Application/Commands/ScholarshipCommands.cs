using MediatR;
using SparshTransactional.Application.DTOs;

namespace SparshTransactional.Application.Commands;

public record CreateScholarshipCommand : IRequest<ScholarshipMasterDto>
{
    public string Name { get; init; } = string.Empty;
    public string? Description { get; init; }
    public string? Type { get; init; }
    public decimal? CoveragePercent { get; init; }
    public decimal? MaxAmount { get; init; }
    public long CreatedBy { get; init; }
}

public record UpdateScholarshipCommand : IRequest<ScholarshipMasterDto>
{
    public long ScholarshipId { get; init; }
    public string Name { get; init; } = string.Empty;
    public string? Description { get; init; }
    public string? Type { get; init; }
    public decimal? CoveragePercent { get; init; }
    public decimal? MaxAmount { get; init; }
    public long UpdatedBy { get; init; }
}

public record DeactivateScholarshipCommand(long ScholarshipId, long UpdatedBy) : IRequest<bool>;

public record AddEligibilityCriteriaCommand : IRequest<EligibilityCriteriaDto>
{
    public long ScholarshipId { get; init; }
    public string CriteriaName { get; init; } = string.Empty;
    public string? CriteriaDescription { get; init; }
    public decimal? MinScore { get; init; }
    public decimal? MaxFamilyIncome { get; init; }
    public long CreatedBy { get; init; }
}

public record SubmitApplicationCommand : IRequest<ScholarshipApplicationDto>
{
    public long StudentId { get; init; }
    public long ScholarshipId { get; init; }
    public decimal? FamilyIncome { get; init; }
    public long CreatedBy { get; init; }
}

public record ApproveApplicationCommand : IRequest<ScholarshipApplicationDto>
{
    public long ApplicationId { get; init; }
    public long ApprovedBy { get; init; }
    public decimal ApprovedAmount { get; init; }
}

public record RejectApplicationCommand : IRequest<ScholarshipApplicationDto>
{
    public long ApplicationId { get; init; }
    public long RejectedBy { get; init; }
    public string? Reason { get; init; }
}

public record CreateDisbursementCommand : IRequest<ScholarshipDisbursementDto>
{
    public long ApplicationId { get; init; }
    public decimal Amount { get; init; }
    public long CreatedBy { get; init; }
}

public record CompleteDisbursementCommand : IRequest<ScholarshipDisbursementDto>
{
    public long DisbursementId { get; init; }
    public string PaymentReference { get; init; } = string.Empty;
}
