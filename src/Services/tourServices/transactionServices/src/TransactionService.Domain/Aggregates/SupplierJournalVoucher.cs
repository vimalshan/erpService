using TransactionService.Domain.Common;
using TransactionService.Domain.Entities;
using TransactionService.Domain.Events;
using TransactionService.Domain.Exceptions;
using TransactionService.Domain.ValueObjects;

namespace TransactionService.Domain.Aggregates;

/// <summary>
/// Aggregate Root - Maps to JVSUP_MAIN with child JVSUP_SUB lines.
/// Supplier Journal Voucher for vendor invoice/credit transactions.
/// </summary>
public sealed class SupplierJournalVoucher : AuditableEntity
{
    private readonly List<SupplierJVLine> _lines = [];

    private SupplierJournalVoucher() { }

    public long JvId { get; private set; }
    public string JvType { get; private set; } = default!;
    public DateTime JvDate { get; private set; }
    public long JvVendorId { get; private set; }
    public string? JvOraRefNo { get; private set; }
    public string JvStatus { get; private set; } = default!;
    public long JvPayUnitId { get; private set; }
    public string JvRefInvNo { get; private set; } = default!;
    public decimal JvNetAmt { get; private set; }
    public string JvTrnType { get; private set; } = default!;
    public long JvOraVendorId { get; private set; }
    public long JvAdminId { get; private set; }
    public long JvInvBatchId { get; private set; }
    public long JvOraSiteId { get; private set; }
    public string JvCenvatApplicable { get; private set; } = default!;
    public string JvDocKeyNo { get; private set; } = default!;

    public IReadOnlyCollection<SupplierJVLine> Lines => _lines.AsReadOnly();

    public static SupplierJournalVoucher Create(
        long jvId, string jvType, DateTime jvDate, long vendorId,
        long payUnitId, string refInvNo, decimal netAmt, string trnType,
        long oraVendorId, long adminId, long invBatchId, long oraSiteId,
        string cenvatApplicable, string docKeyNo, long createdBy)
    {
        var jv = new SupplierJournalVoucher
        {
            JvId = jvId,
            JvType = jvType,
            JvDate = jvDate,
            JvVendorId = vendorId,
            JvStatus = PostingStatus.Pending.Value,
            JvPayUnitId = payUnitId,
            JvRefInvNo = refInvNo,
            JvNetAmt = netAmt,
            JvTrnType = trnType,
            JvOraVendorId = oraVendorId,
            JvAdminId = adminId,
            JvInvBatchId = invBatchId,
            JvOraSiteId = oraSiteId,
            JvCenvatApplicable = cenvatApplicable,
            JvDocKeyNo = docKeyNo,
            CreatedBy = createdBy,
            CreatedOn = DateTime.UtcNow
        };

        jv.RaiseDomainEvent(new SupplierJVCreatedEvent(
            Guid.NewGuid(), jvId, vendorId, jvType, netAmt, createdBy, DateTime.UtcNow));

        return jv;
    }

    public void AddLine(SupplierJVLine line)
    {
        if (JvStatus != PostingStatus.Pending.Value)
            throw new JournalVoucherAlreadyPostedException(JvId);

        _lines.Add(line);
    }

    public void Post(string? oracleRefNo, long postedBy)
    {
        if (JvStatus != PostingStatus.Pending.Value)
            throw new JournalVoucherAlreadyPostedException(JvId);

        if (_lines.Count == 0)
            throw new DomainException($"Cannot post Supplier JV '{JvId}' with no line items.");

        JvStatus = PostingStatus.Posted.Value;
        JvOraRefNo = oracleRefNo;
        ModifiedBy = postedBy;
        ModifiedOn = DateTime.UtcNow;

        RaiseDomainEvent(new SupplierJVPostedEvent(
            Guid.NewGuid(), JvId, oracleRefNo, postedBy, DateTime.UtcNow));
    }

    public void Cancel(long cancelledBy)
    {
        if (JvStatus == PostingStatus.Posted.Value)
            throw new DomainException($"Cannot cancel posted Supplier JV '{JvId}'.");

        JvStatus = PostingStatus.Cancelled.Value;
        ModifiedBy = cancelledBy;
        ModifiedOn = DateTime.UtcNow;
    }
}
