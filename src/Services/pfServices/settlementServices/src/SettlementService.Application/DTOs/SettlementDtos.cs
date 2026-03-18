namespace SettlementService.Application.DTOs;

public record SettlementDto
{
    public long SettlementNumber { get; init; }
    public string? TrustCode { get; init; }
    public long? MemberNo { get; init; }
    public string? SettlementType { get; init; }
    public DateTime? SettlementDate { get; init; }
    public DateTime? DolDate { get; init; }
    public string? Reason { get; init; }
    public DateTime? UpdatedOn { get; init; }
    public long? UpdatedByEmpSysId { get; init; }
    public DateTime? AccountDate { get; init; }
    public long? FinYear { get; init; }
    public string? JvVoucherType { get; init; }
    public long? JvNo { get; init; }
    public string? SetIntFlag { get; init; }
    public string? TaxStatus { get; init; }
    public long? TaxRate { get; init; }
    public decimal? SettlementAmount { get; init; }
    public string? Status { get; init; }
    public List<DeductionDto> Deductions { get; init; } = [];
    public List<ApprovalDto> Approvals { get; init; } = [];
    public List<PaymentDto> Payments { get; init; } = [];
}

public record DeductionDto
{
    public long DeductionId { get; init; }
    public long SettlementNumber { get; init; }
    public string DeductionType { get; init; } = string.Empty;
    public decimal Amount { get; init; }
    public DateTime CreatedOn { get; init; }
}

public record ApprovalDto
{
    public long ApprovalId { get; init; }
    public long SettlementNumber { get; init; }
    public int Level { get; init; }
    public long ApprovedBySysId { get; init; }
    public string Status { get; init; } = string.Empty;
    public string? Remarks { get; init; }
    public DateTime ApprovalDate { get; init; }
}

public record PaymentDto
{
    public long PaymentId { get; init; }
    public long SettlementNumber { get; init; }
    public string PaymentMode { get; init; } = string.Empty;
    public decimal Amount { get; init; }
    public DateTime PaymentDate { get; init; }
    public string? ReferenceNo { get; init; }
    public string Status { get; init; } = string.Empty;
}
