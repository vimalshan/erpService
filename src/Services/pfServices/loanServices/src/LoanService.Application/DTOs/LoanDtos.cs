namespace LoanService.Application.DTOs;

public record LoanDto
{
    public long LoanNo { get; init; }
    public string? TrustCode { get; init; }
    public long? MemberId { get; init; }
    public DateTime? LoanDate { get; init; }
    public decimal? LoanAmount { get; init; }
    public long? LoanType { get; init; }
    public string? LoanReason { get; init; }
    public string? LoanTenure { get; init; }
    public decimal? PrincipalOutstanding { get; init; }
    public string Status { get; init; } = null!;
    public decimal? Rate { get; init; }
    public DateTime? ApprovalDate { get; init; }
    public DateTime? ClosureDate { get; init; }
    public List<RepaymentDto> Repayments { get; init; } = [];
    public List<DeductionDto> Deductions { get; init; } = [];
}

public record RepaymentDto
{
    public long RepayId { get; init; }
    public long LoanNo { get; init; }
    public int InstallmentNo { get; init; }
    public decimal RepayAmount { get; init; }
    public DateTime DueDate { get; init; }
    public DateTime? PaidDate { get; init; }
    public decimal? PaidAmount { get; init; }
    public string Status { get; init; } = null!;
}

public record DeductionDto
{
    public long DedId { get; init; }
    public long LoanNo { get; init; }
    public decimal? ContributionId { get; init; }
    public decimal DedAmount { get; init; }
    public DateTime DedDate { get; init; }
}

public record ActiveLoanDto
{
    public long LoanNo { get; init; }
    public long? MemberId { get; init; }
    public decimal? LoanAmount { get; init; }
    public decimal? PrincipalOutstanding { get; init; }
    public DateTime? LoanDate { get; init; }
    public DateTime? ApprovalDate { get; init; }
    public int RemainingInstallments { get; init; }
}
