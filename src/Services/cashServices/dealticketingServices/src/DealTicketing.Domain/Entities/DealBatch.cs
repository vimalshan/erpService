using DealTicketing.Domain.Common;
using DealTicketing.Domain.Events;
using DealTicketing.Domain.Exceptions;

namespace DealTicketing.Domain.Entities;

/// <summary>Aggregate root — batch header for deal processing.</summary>
public class DealBatch : BaseEntity
{
    public long DealBatchId { get; private set; }
    public DateTime DealDate { get; private set; }
    public long DealDerType { get; private set; }         // FK → LovMaster
    public string? DealScreenshot { get; private set; }
    public long? DealBookedBy { get; private set; }
    public string? DealBankTrader { get; private set; }
    public long? DealBankId { get; private set; }
    public long? DealOptionType { get; private set; }
    public decimal DealBusinessId { get; private set; }
    public char? DealRejStatus { get; private set; }
    public string? DealRejReason { get; private set; }
    public string? DealErrRemarks { get; private set; }
    public decimal DealModifiedBy { get; private set; }
    public DateTime DealModifiedOn { get; private set; }
    public decimal? DealUnitId { get; private set; }

    // Navigation
    public Bank? Bank { get; private set; }
    private readonly List<DealDetail> _dealDetails = [];
    public IReadOnlyCollection<DealDetail> DealDetails => _dealDetails.AsReadOnly();

    private DealBatch() { }

    public DealBatch(
        long batchId, DateTime dealDate, long derType, long? bankId,
        long? bookedBy, string? bankTrader, decimal businessId, decimal modifiedBy, decimal? unitId)
    {
        DealBatchId = batchId;
        DealDate = dealDate;
        DealDerType = derType;
        DealBankId = bankId;
        DealBookedBy = bookedBy;
        DealBankTrader = bankTrader;
        DealBusinessId = businessId;
        DealModifiedBy = modifiedBy;
        DealModifiedOn = DateTime.UtcNow;
        DealUnitId = unitId;

        AddDomainEvent(new DealBatchCreatedEvent(batchId, dealDate, derType));
    }

    public void Reject(string reason, decimal modifiedBy)
    {
        DealRejStatus = 'Y';
        DealRejReason = reason;
        DealModifiedBy = modifiedBy;
        DealModifiedOn = DateTime.UtcNow;

        AddDomainEvent(new DealBatchRejectedEvent(DealBatchId, reason));
    }

    public void SetScreenshot(string screenshot, decimal modifiedBy)
    {
        DealScreenshot = screenshot;
        DealModifiedBy = modifiedBy;
        DealModifiedOn = DateTime.UtcNow;
    }

    public DealDetail AddDealDetail(
        long dealId, long dealNo, long versionId, char? transType,
        decimal? amount, long? currency1, long? currency2,
        decimal? spotRate, decimal? bookRate, DateTime? matDate,
        decimal modifiedBy)
    {
        var detail = new DealDetail(
            dealId, dealNo, versionId, DealBatchId, transType,
            amount, currency1, currency2, spotRate, bookRate, matDate, modifiedBy);
        _dealDetails.Add(detail);
        return detail;
    }
}
