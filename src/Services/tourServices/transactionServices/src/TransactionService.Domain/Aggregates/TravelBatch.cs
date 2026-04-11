using TransactionService.Domain.Common;
using TransactionService.Domain.Entities;
using TransactionService.Domain.Events;
using TransactionService.Domain.Exceptions;
using TransactionService.Domain.ValueObjects;

namespace TransactionService.Domain.Aggregates;

/// <summary>
/// Aggregate Root - Maps to TRAVEL_BATCHMAIN with children TRAVEL_BATCHSUB, TRAVEL_BATCHCC, TRAVEL_BATCHCONTRACT, TRAVEL_BATCHSUBBRK.
/// Travel batch for vendor invoice processing and payment.
/// </summary>
public sealed class TravelBatch : BaseEntity
{
    private readonly List<TravelBatchSub> _subItems = [];

    private TravelBatch() { }

    public string BatchId { get; private set; } = default!;
    public string? AdminId { get; private set; }
    public string? PayUnitId { get; private set; }
    public DateTime? BatchDate { get; private set; }
    public string? InvNum { get; private set; }
    public DateTime? InvDate { get; private set; }
    public string? InvAmount { get; private set; }
    public string? Status { get; private set; }
    public string? AdminRemarks { get; private set; }
    public string? FinanceRemarks { get; private set; }
    public string? VendorId { get; private set; }
    public string? ApprovedAmount { get; private set; }
    public string? BillAmount { get; private set; }
    public string? ServiceTax { get; private set; }
    public string? CessTax { get; private set; }
    public string? AdditionalTax { get; private set; }
    public string? TotalPayable { get; private set; }
    public string? JvId { get; private set; }
    public string? PaymentTerms { get; private set; }
    public DateTime? BillDate { get; private set; }
    public string? BatchType { get; private set; }
    public string? CreatedBy { get; private set; }
    public DateTime? CreatedOn { get; private set; }
    public string? ApprovedBy { get; private set; }
    public DateTime? ApprovedOn { get; private set; }
    public string? FinApprovedBy { get; private set; }
    public DateTime? FinApprovedOn { get; private set; }
    public string? HigherCess { get; private set; }
    public string? RoundingOff { get; private set; }
    public string? CabType { get; private set; }
    public decimal? Surcharge { get; private set; }
    public decimal? BookingCharges { get; private set; }
    public string? CenvatApplicable { get; private set; }
    public string? DocRefNo { get; private set; }
    public string? SourceUid { get; private set; }

    public IReadOnlyCollection<TravelBatchSub> SubItems => _subItems.AsReadOnly();

    public static TravelBatch Create(
        string batchId, string adminId, string payUnitId, string vendorId,
        string? invNum, string? invAmount, string? batchType, string createdBy)
    {
        ArgumentException.ThrowIfNullOrWhiteSpace(batchId);

        var batch = new TravelBatch
        {
            BatchId = batchId,
            AdminId = adminId,
            PayUnitId = payUnitId,
            VendorId = vendorId,
            InvNum = invNum,
            InvAmount = invAmount,
            BatchType = batchType,
            Status = BatchStatus.Pending.Value,
            BatchDate = DateTime.UtcNow,
            CreatedBy = createdBy,
            CreatedOn = DateTime.UtcNow
        };

        batch.RaiseDomainEvent(new TravelBatchCreatedEvent(
            Guid.NewGuid(), batchId, vendorId, adminId, DateTime.UtcNow));

        return batch;
    }

    public void AddSubItem(TravelBatchSub sub) => _subItems.Add(sub);

    public void AdminApprove(string approvedBy, string? approvedAmount, string? remarks = null)
    {
        if (Status != BatchStatus.Pending.Value)
            throw new TravelBatchInvalidStateException(BatchId, Status!, "admin approved");

        Status = BatchStatus.AdminApproved.Value;
        ApprovedBy = approvedBy;
        ApprovedOn = DateTime.UtcNow;
        ApprovedAmount = approvedAmount;
        AdminRemarks = remarks;

        RaiseDomainEvent(new TravelBatchAdminApprovedEvent(
            Guid.NewGuid(), BatchId, approvedBy, DateTime.UtcNow));
    }

    public void FinanceApprove(string approvedBy, string? remarks = null)
    {
        if (Status != BatchStatus.AdminApproved.Value)
            throw new TravelBatchInvalidStateException(BatchId, Status!, "finance approved");

        Status = BatchStatus.FinanceApproved.Value;
        FinApprovedBy = approvedBy;
        FinApprovedOn = DateTime.UtcNow;
        FinanceRemarks = remarks;

        RaiseDomainEvent(new TravelBatchFinanceApprovedEvent(
            Guid.NewGuid(), BatchId, approvedBy, DateTime.UtcNow));
    }

    public void PostJV(string jvId)
    {
        if (Status != BatchStatus.FinanceApproved.Value)
            throw new TravelBatchInvalidStateException(BatchId, Status!, "JV posted");

        Status = BatchStatus.JVPosted.Value;
        JvId = jvId;

        RaiseDomainEvent(new TravelBatchJVPostedEvent(
            Guid.NewGuid(), BatchId, jvId, DateTime.UtcNow));
    }

    public void Reject(string rejectedBy, string? remarks = null)
    {
        if (Status == BatchStatus.JVPosted.Value || Status == BatchStatus.Cancelled.Value)
            throw new TravelBatchInvalidStateException(BatchId, Status!, "rejected");

        Status = BatchStatus.Rejected.Value;
        FinanceRemarks = remarks;

        RaiseDomainEvent(new TravelBatchRejectedEvent(
            Guid.NewGuid(), BatchId, rejectedBy, remarks, DateTime.UtcNow));
    }

    public void Cancel()
    {
        if (Status == BatchStatus.JVPosted.Value)
            throw new TravelBatchInvalidStateException(BatchId, Status!, "cancelled");

        Status = BatchStatus.Cancelled.Value;
    }
}
