namespace ContributionService.Application.DTOs;

public record ContributionMainDto
{
    public long ContributionBatchNo { get; init; }
    public string ContributionTrustCode { get; init; } = null!;
    public string ContributionCategory { get; init; } = null!;
    public string ContributionPayunitCode { get; init; } = null!;
    public DateTime ContributionPayMonthStart { get; init; }
    public DateTime ContributionPayMonthEnd { get; init; }
    public string ContributionStatus { get; init; } = null!;
    public decimal? ContributionJvNo { get; init; }
    public DateTime? ContributionEntOn { get; init; }
    public long ContributionRefNo { get; init; }
    public int MemberCount { get; init; }
    public decimal TotalEeAmount { get; init; }
    public decimal TotalErAmount { get; init; }
    public decimal TotalAmount { get; init; }
}

public record ContributionDetailDto
{
    public decimal ContributionBatchNo { get; init; }
    public decimal ContributionId { get; init; }
    public decimal ContributionMemberNo { get; init; }
    public string ContributionUnitCode { get; init; } = null!;
    public decimal ContributionEmployeeNo { get; init; }
    public decimal? ContributionReferenceNo { get; init; }
    public string? ContributionReferenceRemarks { get; init; }
    public decimal ContributionBasicAmount { get; init; }
    public decimal ContributionFpsBasicAmount { get; init; }
    public decimal ContributionEeAmount { get; init; }
    public decimal ContributionErAmount { get; init; }
    public decimal ContributionVeAmount { get; init; }
    public decimal ContributionFpAmount { get; init; }
    public decimal ContributionLoanPrincipal { get; init; }
    public decimal ContributionLoanInterest { get; init; }
    public string ContributionEntByUserId { get; init; } = null!;
    public DateTime ContributionEntOn { get; init; }
    public string ContributionTypeCode { get; init; } = null!;
}

public record ContributionBreakupDto
{
    public long ContributionBatchNo { get; init; }
    public long ContributionId { get; init; }
    public long ContributionPayTranNo { get; init; }
    public string ContributionEdCode { get; init; } = null!;
    public decimal ContributionPayAmount { get; init; }
    public decimal ContributionEeAmount { get; init; }
    public decimal ContributionErAmount { get; init; }
}

public record SuperannuationBatchDto
{
    public long SnBatchNo { get; init; }
    public long? SnTrustCode { get; init; }
    public string? SnCategory { get; init; }
    public string? SnPayunitCode { get; init; }
    public string? SnPayMonthStart { get; init; }
    public DateTime? SnPayMonthEnd { get; init; }
    public string? SnStatus { get; init; }
    public string? SnConAmt { get; init; }
    public DateTime? SnPayDate { get; init; }
    public int EmployeeCount { get; init; }
    public decimal TotalContribution { get; init; }
}

public record SuperannuationContributionDto
{
    public long SnSlrNum { get; init; }
    public long? SnFinYer { get; init; }
    public decimal? SnPinNum { get; init; }
    public string? SnEmpNam { get; init; }
    public decimal? SnFudNum { get; init; }
    public DateTime? SnConDat { get; init; }
    public decimal? SnUntNos { get; init; }
    public decimal? SnNavAmt { get; init; }
    public decimal? SnConAmt { get; init; }
    public string? SnConTyp { get; init; }
}

public record SuperannuationTrustNameDto
{
    public decimal StFndNum { get; init; }
    public string? StFndNam { get; init; }
}

public record ContributionSummaryDto
{
    public long ContributionBatchNo { get; init; }
    public string TrustCode { get; init; } = null!;
    public string PayunitCode { get; init; } = null!;
    public string Status { get; init; } = null!;
    public int MemberCount { get; init; }
    public decimal TotalEeContribution { get; init; }
    public decimal TotalErContribution { get; init; }
    public decimal TotalContribution { get; init; }
}

public record ProcessContributionResultDto
{
    public long BatchNo { get; init; }
    public int RowsProcessed { get; init; }
    public string Message { get; init; } = null!;
}
