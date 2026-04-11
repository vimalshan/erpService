using TransactionService.Domain.Common;
using TransactionService.Domain.Events;
using TransactionService.Domain.ValueObjects;

namespace TransactionService.Domain.Entities;

/// <summary>
/// Maps to JVEMPPAY_DET - Employee payment details (ADV/EXP/ADJ)
/// </summary>
public sealed class EmployeePayment : AuditableEntity
{
    private EmployeePayment() { }

    public long PayId { get; private set; }
    public long PayTpId { get; private set; }
    public string PayTrnType { get; private set; } = default!;
    public long PayEmpSysId { get; private set; }
    public long PayUnitId { get; private set; }
    public string PayMode { get; private set; } = default!;
    public string PayType { get; private set; } = default!;
    public DateTime? PayDate { get; private set; }
    public decimal PayAmount { get; private set; }
    public long PayRefId { get; private set; }
    public long PayBatchId { get; private set; }
    public long PayJvId { get; private set; }

    public static EmployeePayment Create(
        long payId, long tpId, string trnType, long empSysId,
        long unitId, string mode, string payType, decimal amount,
        long refId, long batchId, long jvId, long createdBy)
    {
        var payment = new EmployeePayment
        {
            PayId = payId,
            PayTpId = tpId,
            PayTrnType = trnType,
            PayEmpSysId = empSysId,
            PayUnitId = unitId,
            PayMode = mode,
            PayType = payType,
            PayAmount = amount,
            PayRefId = refId,
            PayBatchId = batchId,
            PayJvId = jvId,
            CreatedBy = createdBy,
            CreatedOn = DateTime.UtcNow
        };

        payment.RaiseDomainEvent(new EmployeePaymentCreatedEvent(
            Guid.NewGuid(), payId, empSysId, payType, amount, DateTime.UtcNow));

        return payment;
    }
}
