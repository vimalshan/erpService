using TravelRequestService.Domain.Common;

namespace TravelRequestService.Domain.Entities;

public class TravelAdvance : BaseEntity
{
    public long RequestNumber { get; private set; }
    public long AdvanceNumber { get; private set; }
    public DateTime? AdvanceDate { get; private set; }
    public decimal? AdvanceAmount { get; private set; }
    public long? UnitCode { get; private set; }
    public decimal? ApprovedAmount { get; private set; }
    public decimal? PaidAmount { get; private set; }
    public DateTime? PaidDate { get; private set; }
    public decimal? AdjustedAmount { get; private set; }
    public long? PayNumber { get; private set; }
    public string? PayType { get; private set; }
    public string? EmployeeUnit { get; private set; }
    public long? EmployeeNumber { get; private set; }
    public long? TransactionNumber { get; private set; }

    private TravelAdvance() { }

    public static TravelAdvance Create(
        long requestNumber,
        long advanceNumber,
        decimal? advanceAmount,
        long? unitCode,
        long? employeeNumber)
    {
        return new TravelAdvance
        {
            RequestNumber = requestNumber,
            AdvanceNumber = advanceNumber,
            AdvanceDate = DateTime.UtcNow,
            AdvanceAmount = advanceAmount,
            UnitCode = unitCode,
            EmployeeNumber = employeeNumber
        };
    }

    public void MarkAsPaid(decimal paidAmount, string payType)
    {
        PaidAmount = paidAmount;
        PaidDate = DateTime.UtcNow;
        PayType = payType;
    }
}
