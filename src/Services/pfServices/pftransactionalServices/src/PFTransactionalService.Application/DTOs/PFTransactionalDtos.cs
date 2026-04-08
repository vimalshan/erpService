namespace PFTransactionalService.Application.DTOs;

public record PFAccumulationDto
{
    public long PfAccId { get; init; }
    public long EmpSysId { get; init; }
    public long MemberNo { get; init; }
    public string? TrustCode { get; init; }
    public decimal PfAccBal { get; init; }
    public decimal PfEmpContTotal { get; init; }
    public decimal PfErContTotal { get; init; }
    public decimal PfVolContTotal { get; init; }
    public string? PfAccStatus { get; init; }
    public long CreatedBy { get; init; }
    public DateTime CreatedOn { get; init; }
    public long? UpdatedBy { get; init; }
    public DateTime? UpdatedOn { get; init; }
    public List<ContributionTxnDto> Contributions { get; init; } = [];
    public List<WithdrawalCertificateDto> Certificates { get; init; } = [];
}

public record ContributionTxnDto
{
    public long PfTxnId { get; init; }
    public long EmpSysId { get; init; }
    public decimal PfEmpContribution { get; init; }
    public decimal PfErContribution { get; init; }
    public decimal PfVolContribution { get; init; }
    public DateTime PfTxnDate { get; init; }
    public DateTime PfTxnMonth { get; init; }
    public string? PfTxnStatus { get; init; }
    public long CreatedBy { get; init; }
    public DateTime CreatedOn { get; init; }
}

public record PFSettlementDto
{
    public long PfSettlementId { get; init; }
    public long EmpSysId { get; init; }
    public decimal PfSettlementAmount { get; init; }
    public string? PfSettlementType { get; init; }
    public DateTime PfSettlementDate { get; init; }
    public string? PfSettlementStatus { get; init; }
    public long ApprovedBy { get; init; }
    public long CreatedBy { get; init; }
    public DateTime CreatedOn { get; init; }
    public List<SettlementTxnDto> Transactions { get; init; } = [];
}

public record SettlementTxnDto
{
    public long PfSettlementTxnId { get; init; }
    public long PfSettlementId { get; init; }
    public long EmpSysId { get; init; }
    public decimal PfSettlementTxnAmount { get; init; }
    public DateTime PfSettlementTxnDate { get; init; }
    public string? PfSettlementTxnStatus { get; init; }
}

public record WithdrawalCertificateDto
{
    public long CertificateId { get; init; }
    public long PfSettlementId { get; init; }
    public long EmpSysId { get; init; }
    public decimal CertificateAmount { get; init; }
    public DateTime CertificateDate { get; init; }
    public string? CertificateStatus { get; init; }
}

public record FinancialYearDto
{
    public long AcSrlNum { get; init; }
    public DateTime AcStrDat { get; init; }
    public DateTime AcEndDat { get; init; }
    public string? AcClsFlg { get; init; }
    public string? AcRemarks { get; init; }
    public string? AcIntFlg { get; init; }
}
