using SettlementService.Domain.Common;
using SettlementService.Domain.Enums;

namespace SettlementService.Domain.Entities;

public class SettlementPayment : BaseEntity
{
    public long PayId { get; private set; }
    public long SetNum { get; private set; }
    public string PayMode { get; private set; } = string.Empty;
    public decimal PayAmount { get; private set; }
    public DateTime PayDate { get; private set; }
    public string? PayRefNo { get; private set; }
    public PaymentStatus PayStatus { get; private set; }

    private SettlementPayment() { }

    public SettlementPayment(long setNum, string payMode, decimal payAmount, string? payRefNo = null)
    {
        SetNum = setNum;
        PayMode = payMode ?? throw new ArgumentNullException(nameof(payMode));
        PayAmount = payAmount;
        PayDate = DateTime.UtcNow;
        PayRefNo = payRefNo;
        PayStatus = PaymentStatus.Pending;
    }

    public void MarkCompleted(string? referenceNo = null)
    {
        PayStatus = PaymentStatus.Completed;
        if (referenceNo != null)
            PayRefNo = referenceNo;
    }

    public void MarkFailed()
    {
        PayStatus = PaymentStatus.Failed;
    }
}
