using LoanDefinition.SharedKernel;

namespace LoanDefinition.Domain.Entities;

public class LoanLimitRangeMaster : BaseEntity<long>
{
    public long LoanId { get; private set; }
    public long MinYear { get; private set; }
    public long MaxYear { get; private set; }
    public decimal LoanAmount { get; private set; }
    public DateTime EffectiveDate { get; private set; }
    public DateTime? ClosureDate { get; private set; }
    public long CreatedBy { get; private set; }
    public DateTime CreatedOn { get; private set; }
    public long ModifiedBy { get; private set; }
    public DateTime ModifiedOn { get; private set; }
    public decimal InterestRate { get; private set; }
    public decimal? AdditionalMinValue { get; private set; }

    public LoanMaster? Loan { get; private set; }

    private LoanLimitRangeMaster() { }

    public static LoanLimitRangeMaster Create(
        long id, long loanId, long minYear, long maxYear, decimal loanAmount,
        DateTime effectiveDate, decimal interestRate, long createdBy)
    {
        return new LoanLimitRangeMaster
        {
            Id = id,
            LoanId = loanId,
            MinYear = minYear,
            MaxYear = maxYear,
            LoanAmount = loanAmount,
            EffectiveDate = effectiveDate,
            InterestRate = interestRate,
            CreatedBy = createdBy,
            CreatedOn = DateTime.UtcNow,
            ModifiedBy = createdBy,
            ModifiedOn = DateTime.UtcNow
        };
    }
}
