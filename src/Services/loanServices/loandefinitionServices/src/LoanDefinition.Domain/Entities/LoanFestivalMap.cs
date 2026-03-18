using LoanDefinition.SharedKernel;

namespace LoanDefinition.Domain.Entities;

public class LoanFestivalMap : BaseEntity<long>
{
    public long LoanId { get; private set; }
    public long FestivalId { get; private set; }
    public long ModifiedBy { get; private set; }
    public DateTime ModifiedOn { get; private set; }

    public LoanMaster? Loan { get; private set; }
    public LoanFestival? Festival { get; private set; }

    private LoanFestivalMap() { }

    public static LoanFestivalMap Create(long id, long loanId, long festivalId, long modifiedBy)
    {
        return new LoanFestivalMap
        {
            Id = id,
            LoanId = loanId,
            FestivalId = festivalId,
            ModifiedBy = modifiedBy,
            ModifiedOn = DateTime.UtcNow
        };
    }
}
