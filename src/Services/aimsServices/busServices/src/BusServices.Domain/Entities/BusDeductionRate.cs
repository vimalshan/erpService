using BusServices.Domain.Common;
using BusServices.Domain.Exceptions;

namespace BusServices.Domain.Entities;

/// <summary>Maps to BUSDEDUCTION_RATEMAST table.</summary>
public sealed class BusDeductionRate : BaseEntity
{
    public int DeductId { get; private set; }
    public int BusId { get; private set; }
    public decimal Amount { get; private set; }
    public DateTime EffectiveDate { get; private set; }
    public DateTime? ClosingDate { get; private set; }
    public long LastModifiedBy { get; private set; }
    public DateTime LastModifiedOn { get; private set; }

    private BusDeductionRate() { }

    public static BusDeductionRate Create(int deductId, int busId, decimal amount, DateTime effectiveDate, long createdBy)
    {
        if (amount < 0) throw new DomainException("Deduction amount cannot be negative.");
        if (busId <= 0) throw new DomainException("Invalid bus ID.");

        return new BusDeductionRate
        {
            DeductId = deductId,
            BusId = busId,
            Amount = amount,
            EffectiveDate = effectiveDate.Date,
            LastModifiedBy = createdBy,
            LastModifiedOn = DateTime.UtcNow
        };
    }

    public void Close(DateTime closingDate, long modifiedBy)
    {
        if (closingDate < EffectiveDate)
            throw new DomainException("Closing date cannot be before effective date.");

        ClosingDate = closingDate;
        LastModifiedBy = modifiedBy;
        LastModifiedOn = DateTime.UtcNow;
    }
}
