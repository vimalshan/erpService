namespace ContributionService.Domain.Entities;

public class ContributionTemp
{
    public long ContributionBatchNo { get; set; }
    public long ContributionId { get; set; }
    public long ContributionMemberNo { get; set; }
    public string ContributionUnitCode { get; set; } = null!;
    public int ContributionEmployeeNo { get; set; }
    public int? ContributionReferenceNo { get; set; }
    public string? ContributionReferenceRemarks { get; set; }
    public decimal ContributionBasicAmount { get; set; }
    public decimal ContributionFpsBasicAmount { get; set; }
    public decimal ContributionEeAmount { get; set; }
    public decimal ContributionErAmount { get; set; }
    public decimal ContributionVeAmount { get; set; }
    public decimal ContributionFpAmount { get; set; }
    public decimal ContributionLoanPrincipal { get; set; }
    public decimal ContributionLoanInterest { get; set; }
    public string ContributionEntByUserId { get; set; } = null!;
    public int ContributionEntEmpSysId { get; set; }
    public DateTime ContributionEntOn { get; set; }
    public string ContributionTypeCode { get; set; } = null!;
}
