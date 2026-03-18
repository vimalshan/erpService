using DealTicketing.Domain.Common;
using DealTicketing.Domain.Events;
using DealTicketing.Domain.Exceptions;

namespace DealTicketing.Domain.Entities;

/// <summary>Individual deal transaction record.</summary>
public class DealDetail : BaseEntity
{
    public long DealId { get; private set; }
    public long DealNo { get; private set; }
    public long DealVersionId { get; private set; }
    public long DealBatchId { get; private set; }
    public char? DealTranType { get; private set; }        // B/S/P/C
    public string? DealPosition { get; private set; }      // BC/BP/SP/SC
    public DateTime? DealEntryDate { get; private set; }
    public decimal? DealAmount { get; private set; }
    public long? DealBankId { get; private set; }
    public long? DealCurrency1 { get; private set; }
    public long? DealCurrency2 { get; private set; }
    public decimal? DealSpotRate { get; private set; }
    public decimal? DealForPoints { get; private set; }
    public decimal? DealBankMargin { get; private set; }
    public decimal? DealBookRate { get; private set; }
    public DateTime? DealMatDate { get; private set; }
    public long? DealDealType { get; private set; }
    public long? DealBusiness { get; private set; }
    public long? DealCategory { get; private set; }
    public long? DealStrikePrice { get; private set; }
    public long? DealPplMitOut { get; private set; }
    public char? DealAppStatus { get; private set; }       // Y/N/R/P
    public string? DealAppRemarks { get; private set; }
    public string? DealErrRemarks { get; private set; }
    public string? DealCorrectness { get; private set; }
    public char? DealSigned { get; private set; }
    public long? DealAppBusiness { get; private set; }
    public string? DealDealConfNo { get; private set; }
    public DateTime? DealModifiedOn { get; private set; }
    public decimal? DealModifiedBy { get; private set; }
    public string? DealRemarks { get; private set; }
    public string? DealIrLoan { get; private set; }
    public string? DealIrType { get; private set; }        // IRS/CAP/FLOOR
    public DateTime? DealStartDate { get; private set; }
    public decimal? DealNotPrincipal { get; private set; }
    public string? DealIrsType { get; private set; }       // PAYFIXRECFLOAT/PAYFLOATRECFIX
    public decimal? DealToPay { get; private set; }
    public decimal? DealToRec { get; private set; }
    public string? DealRateScreenshot { get; private set; }
    public long? DealRatePer { get; private set; }
    public decimal? DealLoanAmt { get; private set; }
    public long? DealLoanCurrency { get; private set; }
    public decimal? DealSetAmt { get; private set; }
    public decimal? DealCanAmt { get; private set; }
    public decimal? DealRollAmt { get; private set; }
    public char? DealSetStatus { get; private set; }      // L=Live, C=Closed
    public decimal? DealUnitId { get; private set; }
    public decimal? DealNetBasisPoint { get; private set; }
    public decimal? DealRolloverDealNo { get; private set; }
    public decimal? DealBookingCharges { get; private set; }
    public char? DealSentToBank { get; private set; }

    // Navigation
    public DealBatch DealBatch { get; private set; } = default!;
    public Bank? Bank { get; private set; }
    private readonly List<DealLoanSchedule> _loanSchedules = [];
    private readonly List<DealSettlement> _settlements = [];
    private readonly List<DealAttachment> _attachments = [];
    public IReadOnlyCollection<DealLoanSchedule> LoanSchedules => _loanSchedules.AsReadOnly();
    public IReadOnlyCollection<DealSettlement> Settlements => _settlements.AsReadOnly();
    public IReadOnlyCollection<DealAttachment> Attachments => _attachments.AsReadOnly();

    private DealDetail() { }

    internal DealDetail(
        long dealId, long dealNo, long versionId, long batchId,
        char? tranType, decimal? amount, long? currency1, long? currency2,
        decimal? spotRate, decimal? bookRate, DateTime? matDate, decimal modifiedBy)
    {
        DealId = dealId;
        DealNo = dealNo;
        DealVersionId = versionId;
        DealBatchId = batchId;
        DealTranType = tranType;
        DealAmount = amount;
        DealCurrency1 = currency1;
        DealCurrency2 = currency2;
        DealSpotRate = spotRate;
        DealBookRate = bookRate;
        DealMatDate = matDate;
        DealAppStatus = 'P';    // pending
        DealEntryDate = DateTime.UtcNow;
        DealModifiedBy = modifiedBy;
        DealModifiedOn = DateTime.UtcNow;

        AddDomainEvent(new DealCreatedEvent(dealId, batchId, amount, matDate));
    }

    public void Approve(long appBusiness, string? remarks, decimal modifiedBy)
    {
        if (DealAppStatus == 'Y')
            throw new InvalidDealStatusTransitionException(DealAppStatus.ToString()!, "Y");

        DealAppStatus = 'Y';
        DealAppBusiness = appBusiness;
        DealAppRemarks = remarks;
        DealModifiedBy = modifiedBy;
        DealModifiedOn = DateTime.UtcNow;

        AddDomainEvent(new DealApprovedEvent(DealId, DealBatchId, appBusiness));
    }

    public void Reject(string remarks, decimal modifiedBy)
    {
        DealAppStatus = 'R';
        DealAppRemarks = remarks;
        DealModifiedBy = modifiedBy;
        DealModifiedOn = DateTime.UtcNow;

        AddDomainEvent(new DealRejectedEvent(DealId, DealBatchId, remarks));
    }

    public DealSettlement AddSettlement(
        long setId, decimal gainLossAmt, char? setType,
        decimal? spotRate, decimal? exchangeRate, long? modifiedBy)
    {
        if (DealSetStatus == 'C')
            throw new DealAlreadySettledException(DealId);

        var settlement = new DealSettlement(setId, DealId, gainLossAmt, setType, spotRate, exchangeRate, modifiedBy);
        _settlements.Add(settlement);
        DealSetStatus = 'L';
        DealModifiedOn = DateTime.UtcNow;

        AddDomainEvent(new DealSettledEvent(DealId, setId, gainLossAmt));
        return settlement;
    }

    public void MarkClosed(decimal modifiedBy)
    {
        DealSetStatus = 'C';
        DealModifiedBy = modifiedBy;
        DealModifiedOn = DateTime.UtcNow;
    }
}
