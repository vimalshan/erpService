using travelTransactionService.Domain.Common;

namespace travelTransactionService.Domain.Entities;

public class BatchSubBreakup : BaseEntity
{
    public long SlNo { get; private set; }
    public decimal BookingNumber { get; private set; }
    public string CostUnit { get; private set; } = null!;
    public string CostCode { get; private set; } = null!;
    public string? ProductCode { get; private set; }
    public string? SubAccountCode { get; private set; }

    private BatchSubBreakup() { }

    public static BatchSubBreakup Create(
        long slNo,
        decimal bookingNumber,
        string costUnit,
        string costCode,
        string? productCode = null,
        string? subAccountCode = null)
    {
        return new BatchSubBreakup
        {
            SlNo = slNo,
            BookingNumber = bookingNumber,
            CostUnit = costUnit,
            CostCode = costCode,
            ProductCode = productCode,
            SubAccountCode = subAccountCode
        };
    }
}
