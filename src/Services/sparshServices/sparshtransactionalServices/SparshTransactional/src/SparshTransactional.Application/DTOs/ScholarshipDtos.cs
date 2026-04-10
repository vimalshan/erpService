namespace SparshTransactional.Application.DTOs;

public record ScholarshipMasterDto
{
    public long ScholarshipId { get; init; }
    public string ScholarshipName { get; init; } = string.Empty;
    public string? ScholarshipDescription { get; init; }
    public string? ScholarshipType { get; init; }
    public decimal? CoveragePercent { get; init; }
    public decimal? MaxAmount { get; init; }
    public string Status { get; init; } = "A";
    public long CreatedBy { get; init; }
    public DateTime CreatedOn { get; init; }
    public long? UpdatedBy { get; init; }
    public DateTime? UpdatedOn { get; init; }
    public List<EligibilityCriteriaDto> EligibilityCriteria { get; init; } = [];
}

public record EligibilityCriteriaDto
{
    public long CriteriaId { get; init; }
    public long ScholarshipId { get; init; }
    public string CriteriaName { get; init; } = string.Empty;
    public string? CriteriaDescription { get; init; }
    public decimal? MinScore { get; init; }
    public decimal? MaxFamilyIncome { get; init; }
    public string EligibilityStatus { get; init; } = "A";
    public long CreatedBy { get; init; }
    public DateTime CreatedOn { get; init; }
}

public record ScholarshipApplicationDto
{
    public long ApplicationId { get; init; }
    public long StudentId { get; init; }
    public long ScholarshipId { get; init; }
    public DateTime ApplicationDate { get; init; }
    public decimal? FamilyIncome { get; init; }
    public string ApplicationStatus { get; init; } = "S";
    public decimal? ApprovedAmount { get; init; }
    public long? ApprovedBy { get; init; }
    public string? RejectionReason { get; init; }
    public long CreatedBy { get; init; }
    public DateTime CreatedOn { get; init; }
    public DateTime? UpdatedOn { get; init; }
    public List<ScholarshipDisbursementDto> Disbursements { get; init; } = [];
}

public record ScholarshipDisbursementDto
{
    public long DisbursementId { get; init; }
    public long ApplicationId { get; init; }
    public long StudentId { get; init; }
    public long ScholarshipId { get; init; }
    public decimal DisbursementAmount { get; init; }
    public DateTime? DisbursementDate { get; init; }
    public string DisbursementStatus { get; init; } = "P";
    public string? PaymentReference { get; init; }
    public long CreatedBy { get; init; }
    public DateTime CreatedOn { get; init; }
    public DateTime? UpdatedOn { get; init; }
}
