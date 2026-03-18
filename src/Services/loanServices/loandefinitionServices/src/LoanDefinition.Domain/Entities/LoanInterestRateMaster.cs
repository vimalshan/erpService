using LoanDefinition.SharedKernel;

namespace LoanDefinition.Domain.Entities;

public class LoanInterestRateMaster : BaseEntity<long>
{
    public long LoanId { get; private set; }
    public DateTime EffectiveDate { get; private set; }
    public DateTime? ClosureDate { get; private set; }
    public int Rate { get; private set; }
    public long LastModifiedBy { get; private set; }
    public DateTime LastModifiedOn { get; private set; }
    public long EmiAmount { get; private set; }
    public int InstallmentNos { get; private set; }
    public string RangeSpecific { get; private set; } = "N";

    public LoanMaster? Loan { get; private set; }

    private LoanInterestRateMaster() { }

    public static LoanInterestRateMaster Create(
        long id, long loanId, DateTime effectiveDate, int rate,
        long emiAmount, int installmentNos, string rangeSpecific, long modifiedBy)
    {
        return new LoanInterestRateMaster
        {
            Id = id,
            LoanId = loanId,
            EffectiveDate = effectiveDate,
            Rate = rate,
            EmiAmount = emiAmount,
            InstallmentNos = installmentNos,
            RangeSpecific = rangeSpecific,
            LastModifiedBy = modifiedBy,
            LastModifiedOn = DateTime.UtcNow
        };
    }

    public void Update(int rate, long emiAmount, int installmentNos, DateTime? closureDate, long modifiedBy)
    {
        Rate = rate;
        EmiAmount = emiAmount;
        InstallmentNos = installmentNos;
        ClosureDate = closureDate;
        LastModifiedBy = modifiedBy;
        LastModifiedOn = DateTime.UtcNow;
    }
}
