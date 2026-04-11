using TransactionService.Domain.Common;
using TransactionService.Domain.Entities;
using TransactionService.Domain.Events;
using TransactionService.Domain.Exceptions;
using TransactionService.Domain.ValueObjects;

namespace TransactionService.Domain.Aggregates;

/// <summary>
/// Aggregate Root - Maps to JVEMP_MAIN with child JVEMP_SUB lines.
/// Employee Journal Voucher for travel advance/expense/adjustment transactions.
/// </summary>
public sealed class EmployeeJournalVoucher : AuditableEntity
{
    private readonly List<EmployeeJVLine> _lines = [];

    private EmployeeJournalVoucher() { }

    public long JvBatchId { get; private set; }
    public long JvTpId { get; private set; }
    public string JvType { get; private set; } = default!;
    public DateTime JvDate { get; private set; }
    public long JvEmpSysId { get; private set; }
    public string JvStatus { get; private set; } = default!;
    public string JvTrnType { get; private set; } = default!;
    public string? JvOraRefNo { get; private set; }
    public decimal JvNetAmt { get; private set; }
    public long JvPayUnitId { get; private set; }
    public long? JvTrnRefNo { get; private set; }

    public IReadOnlyCollection<EmployeeJVLine> Lines => _lines.AsReadOnly();

    public static EmployeeJournalVoucher Create(
        long jvBatchId, long tpId, string jvType, DateTime jvDate,
        long empSysId, string trnType, decimal netAmt, long payUnitId,
        long createdBy)
    {
        var jv = new EmployeeJournalVoucher
        {
            JvBatchId = jvBatchId,
            JvTpId = tpId,
            JvType = JournalVoucherType.From(jvType).Value,
            JvDate = jvDate,
            JvEmpSysId = empSysId,
            JvStatus = PostingStatus.Pending.Value,
            JvTrnType = trnType,
            JvNetAmt = netAmt,
            JvPayUnitId = payUnitId,
            CreatedBy = createdBy,
            CreatedOn = DateTime.UtcNow
        };

        jv.RaiseDomainEvent(new EmployeeJVCreatedEvent(
            Guid.NewGuid(), jvBatchId, empSysId, jvType, netAmt, createdBy, DateTime.UtcNow));

        return jv;
    }

    public void AddLine(EmployeeJVLine line)
    {
        if (JvStatus != PostingStatus.Pending.Value)
            throw new JournalVoucherAlreadyPostedException(JvBatchId);

        _lines.Add(line);
    }

    public void Post(string? oracleRefNo, long postedBy)
    {
        if (JvStatus != PostingStatus.Pending.Value)
            throw new JournalVoucherAlreadyPostedException(JvBatchId);

        if (_lines.Count == 0)
            throw new DomainException($"Cannot post JV '{JvBatchId}' with no line items.");

        JvStatus = PostingStatus.Posted.Value;
        JvOraRefNo = oracleRefNo;
        ModifiedBy = postedBy;
        ModifiedOn = DateTime.UtcNow;

        RaiseDomainEvent(new EmployeeJVPostedEvent(
            Guid.NewGuid(), JvBatchId, oracleRefNo, postedBy, DateTime.UtcNow));
    }

    public void Reverse(long reversedBy)
    {
        if (JvStatus != PostingStatus.Posted.Value)
            throw new DomainException($"JV '{JvBatchId}' can only be reversed from Posted status.");

        JvStatus = PostingStatus.Reversed.Value;
        ModifiedBy = reversedBy;
        ModifiedOn = DateTime.UtcNow;

        RaiseDomainEvent(new EmployeeJVReversedEvent(
            Guid.NewGuid(), JvBatchId, reversedBy, DateTime.UtcNow));
    }

    public void Cancel(long cancelledBy)
    {
        if (JvStatus == PostingStatus.Posted.Value)
            throw new DomainException($"Cannot cancel posted JV '{JvBatchId}'. Reverse it first.");

        JvStatus = PostingStatus.Cancelled.Value;
        ModifiedBy = cancelledBy;
        ModifiedOn = DateTime.UtcNow;
    }
}
