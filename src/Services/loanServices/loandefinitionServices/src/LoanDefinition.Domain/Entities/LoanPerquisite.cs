using LoanDefinition.SharedKernel;

namespace LoanDefinition.Domain.Entities;

public class LoanPerquisite : BaseEntity<long>
{
    public string ClassId { get; private set; } = string.Empty;
    public DateTime EffectiveDate { get; private set; }
    public DateTime? ClosureDate { get; private set; }
    public int ItInterestRate { get; private set; }
    public long ModifiedBy { get; private set; }
    public DateTime ModifiedOn { get; private set; }
    public decimal MinAmount { get; private set; }

    private LoanPerquisite() { }

    public static LoanPerquisite Create(long id, string classId, DateTime effectiveDate,
        int itInterestRate, decimal minAmount, long modifiedBy)
    {
        return new LoanPerquisite
        {
            Id = id,
            ClassId = classId,
            EffectiveDate = effectiveDate,
            ItInterestRate = itInterestRate,
            MinAmount = minAmount,
            ModifiedBy = modifiedBy,
            ModifiedOn = DateTime.UtcNow
        };
    }
}
