using DealTicketing.Domain.Common;

namespace DealTicketing.Domain.Entities;

/// <summary>Bank master data for counter-parties.</summary>
public class Bank : BaseEntity
{
    public long BankId { get; private set; }
    public string BankName { get; private set; } = default!;
    public DateTime BankEffDate { get; private set; }
    public DateTime? BankClsDate { get; private set; }
    public decimal BankModifiedBy { get; private set; }
    public DateTime BankModifiedOn { get; private set; }

    // EF navigation
    public ICollection<DealBatch> DealBatches { get; private set; } = [];
    public ICollection<DealDetail> DealDetails { get; private set; } = [];

    private Bank() { }

    public Bank(long bankId, string bankName, DateTime effDate, decimal modifiedBy)
    {
        BankId = bankId;
        BankName = bankName;
        BankEffDate = effDate;
        BankModifiedBy = modifiedBy;
        BankModifiedOn = DateTime.UtcNow;
    }

    public void Close(decimal modifiedBy)
    {
        BankClsDate = DateTime.UtcNow;
        BankModifiedBy = modifiedBy;
        BankModifiedOn = DateTime.UtcNow;
    }
}
