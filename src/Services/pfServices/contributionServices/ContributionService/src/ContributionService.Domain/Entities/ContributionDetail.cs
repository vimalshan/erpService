namespace ContributionService.Domain.Entities;

public class ContributionDetail : BaseEntity
{
    public decimal ContributionBatchNo { get; private set; }
    public decimal ContributionId { get; private set; }
    public decimal ContributionMemberNo { get; private set; }
    public string ContributionUnitCode { get; private set; } = null!;
    public decimal ContributionEmployeeNo { get; private set; }
    public decimal? ContributionReferenceNo { get; private set; }
    public string? ContributionReferenceRemarks { get; private set; }
    public decimal ContributionBasicAmount { get; private set; }
    public decimal ContributionFpsBasicAmount { get; private set; }
    public decimal ContributionEeAmount { get; private set; }
    public decimal ContributionErAmount { get; private set; }
    public decimal ContributionVeAmount { get; private set; }
    public decimal ContributionFpAmount { get; private set; }
    public decimal ContributionLoanPrincipal { get; private set; }
    public decimal ContributionLoanInterest { get; private set; }
    public string ContributionEntByUserId { get; private set; } = null!;
    public decimal ContributionEntEmpSysId { get; private set; }
    public DateTime ContributionEntOn { get; private set; }
    public string ContributionTypeCode { get; private set; } = null!;
    public decimal? ContributionEmpSysId { get; private set; }

    public ContributionMain? Batch { get; private set; }

    private readonly List<ContributionBreakup> _breakups = [];
    public IReadOnlyCollection<ContributionBreakup> Breakups => _breakups.AsReadOnly();

    private ContributionDetail() { }

    public static ContributionDetail Create(
        decimal batchNo, decimal id, decimal memberNo, string unitCode,
        decimal employeeNo, decimal basicAmount, decimal fpsBasicAmount,
        decimal eeAmount, decimal erAmount, decimal veAmount, decimal fpAmount,
        decimal loanPrincipal, decimal loanInterest,
        string entByUserId, decimal entEmpSysId, string typeCode)
    {
        return new ContributionDetail
        {
            ContributionBatchNo = batchNo,
            ContributionId = id,
            ContributionMemberNo = memberNo,
            ContributionUnitCode = unitCode,
            ContributionEmployeeNo = employeeNo,
            ContributionBasicAmount = basicAmount,
            ContributionFpsBasicAmount = fpsBasicAmount,
            ContributionEeAmount = eeAmount,
            ContributionErAmount = erAmount,
            ContributionVeAmount = veAmount,
            ContributionFpAmount = fpAmount,
            ContributionLoanPrincipal = loanPrincipal,
            ContributionLoanInterest = loanInterest,
            ContributionEntByUserId = entByUserId,
            ContributionEntEmpSysId = entEmpSysId,
            ContributionEntOn = DateTime.UtcNow,
            ContributionTypeCode = typeCode
        };
    }

    public void Validate()
    {
        if (ContributionEeAmount < 0 || ContributionErAmount < 0)
            throw new InvalidOperationException("Contribution amounts cannot be negative.");

        var total = ContributionEeAmount + ContributionErAmount;
        if (total > ContributionBasicAmount * 2)
            throw new InvalidOperationException("Total contribution exceeds reasonable threshold.");

        ContributionTypeCode = "V";
    }
}
