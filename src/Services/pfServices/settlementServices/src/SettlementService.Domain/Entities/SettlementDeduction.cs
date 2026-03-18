using SettlementService.Domain.Common;

namespace SettlementService.Domain.Entities;

public class SettlementDeduction : BaseEntity
{
    public long SetDedId { get; private set; }
    public long SetNum { get; private set; }
    public string DedType { get; private set; } = string.Empty;
    public decimal DedAmount { get; private set; }
    public DateTime CreatedOn { get; private set; }

    private SettlementDeduction() { }

    public SettlementDeduction(long setNum, string dedType, decimal dedAmount)
    {
        SetNum = setNum;
        DedType = dedType ?? throw new ArgumentNullException(nameof(dedType));
        DedAmount = dedAmount;
        CreatedOn = DateTime.UtcNow;
    }

    public void UpdateAmount(decimal newAmount)
    {
        DedAmount = newAmount;
    }
}
