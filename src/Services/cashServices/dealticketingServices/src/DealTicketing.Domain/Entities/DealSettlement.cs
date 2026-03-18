using DealTicketing.Domain.Common;

namespace DealTicketing.Domain.Entities;

public class DealSettlement : BaseEntity
{
    public long SetId { get; private set; }
    public long SetDealId { get; private set; }
    public decimal? SetSpotRate { get; private set; }
    public DateTime? SetDate { get; private set; }
    public string? SetMoneyType { get; private set; }      // IN/OUT
    public char? SetExcType { get; private set; }          // Y/N
    public decimal SetGainLossAmt { get; private set; }
    public char? SetType { get; private set; }             // U/C/R
    public DateTime? SetCanDate { get; private set; }
    public decimal? SetPremiumRate { get; private set; }
    public decimal? SetPremiumAmount { get; private set; }
    public long? SetIrDays { get; private set; }
    public DateTime? SetIrStartDate { get; private set; }
    public decimal? SetIrAmount { get; private set; }
    public decimal? SetWindFee { get; private set; }
    public decimal? SetWindRate { get; private set; }
    public decimal? SetAmount { get; private set; }
    public decimal? SetCreditDebit { get; private set; }
    public long? SetModifiedBy { get; private set; }
    public DateTime? SetModifiedOn { get; private set; }
    public decimal? SetExchangeRate { get; private set; }
    public decimal? SetActGainLossAmt { get; private set; }
    public DateTime? SetDcDate { get; private set; }
    public decimal? SetDcAmnt { get; private set; }
    public string? SetBankName { get; private set; }
    public string? SetBankAcNo { get; private set; }

    public DealDetail DealDetail { get; private set; } = default!;
    public ICollection<DealSettlementAttachment> Attachments { get; private set; } = [];

    private DealSettlement() { }

    internal DealSettlement(
        long setId, long dealId, decimal gainLossAmt,
        char? setType, decimal? spotRate, decimal? exchangeRate, long? modifiedBy)
    {
        SetId = setId;
        SetDealId = dealId;
        SetGainLossAmt = gainLossAmt;
        SetType = setType;
        SetSpotRate = spotRate;
        SetExchangeRate = exchangeRate;
        SetDate = DateTime.UtcNow;
        SetModifiedBy = modifiedBy;
        SetModifiedOn = DateTime.UtcNow;
    }
}
