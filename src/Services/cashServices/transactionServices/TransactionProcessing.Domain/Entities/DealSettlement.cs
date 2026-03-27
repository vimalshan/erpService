using TransactionProcessing.Domain.Common;
using TransactionProcessing.Domain.Events;

namespace TransactionProcessing.Domain.Entities;

public class DealSettlement : BaseEntity
{
    public long SettlementId { get; private set; }
    public long TxnId { get; private set; }
    public long DealId { get; private set; }
    public long SetId { get; private set; }
    public char SettlementType { get; private set; }       // U=Utilized, C=Cancelled, R=Rollover
    public decimal? SpotRate { get; private set; }
    public decimal? ExchangeRate { get; private set; }
    public decimal SettlementAmount { get; private set; }
    public decimal? GainLossAmount { get; private set; }
    public decimal? PremiumAmount { get; private set; }
    public decimal? WindingFee { get; private set; }
    public decimal NetAmount { get; private set; }
    public long? BankAccountId { get; private set; }
    public string ProcessingStatus { get; private set; } = "PENDING";
    public long CreatedBy { get; private set; }
    public DateTime CreatedOn { get; private set; }

    public FinancialTransaction? Transaction { get; private set; }

    private DealSettlement() { }

    public static DealSettlement Create(
        long txnId, long dealId, long setId, char settlementType,
        decimal? spotRate, decimal? exchangeRate, decimal settlementAmount,
        decimal? gainLossAmount, decimal? premiumAmount, decimal? windingFee,
        decimal netAmount, long? bankAccountId, long createdBy)
    {
        var settlement = new DealSettlement
        {
            TxnId = txnId,
            DealId = dealId,
            SetId = setId,
            SettlementType = settlementType,
            SpotRate = spotRate,
            ExchangeRate = exchangeRate,
            SettlementAmount = settlementAmount,
            GainLossAmount = gainLossAmount,
            PremiumAmount = premiumAmount,
            WindingFee = windingFee,
            NetAmount = netAmount,
            BankAccountId = bankAccountId,
            ProcessingStatus = "PENDING",
            CreatedBy = createdBy,
            CreatedOn = DateTime.UtcNow
        };

        settlement.AddDomainEvent(new SettlementProcessedEvent(
            settlement.SettlementId, dealId, setId, settlementType, netAmount));
        return settlement;
    }

    public void MarkProcessed() => ProcessingStatus = "COMPLETED";
    public void MarkFailed() => ProcessingStatus = "FAILED";
}
