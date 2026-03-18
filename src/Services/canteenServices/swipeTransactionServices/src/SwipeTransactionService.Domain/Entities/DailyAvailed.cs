using SwipeTransactionService.Domain.Common;
using SwipeTransactionService.Domain.Events;

namespace SwipeTransactionService.Domain.Entities;

/// <summary>
/// Represents a canteen daily availed record.
/// Maps to CANTEEN_DAYWISE_AVAILED.
/// </summary>
public sealed class DailyAvailed : BaseEntity
{
    public long SerialNumber { get; private set; }
    public long CompanyCode { get; private set; }
    public long EmployeeSysId { get; private set; }
    public char? EmployeeType { get; private set; }
    public string? SwipeDate { get; private set; }
    public long? ItemCode { get; private set; }
    public char? ItemType { get; private set; }
    public decimal? EmployeeContribution { get; private set; }
    public decimal? EmployerContribution { get; private set; }
    public string? CanteenNumber { get; private set; }
    public long? ItemQuantity { get; private set; }
    public long? EntryUser { get; private set; }
    public string? EntryDate { get; private set; }
    public string? FlexField1 { get; private set; }
    public string? GradeCategory { get; private set; }

    private DailyAvailed() { }

    public static DailyAvailed Create(
        long serialNumber,
        long companyCode,
        long empSysId,
        long itemCode,
        decimal employeeContribution,
        decimal employerContribution,
        long itemQuantity,
        long entryUser,
        string canteenNumber)
    {
        var availed = new DailyAvailed
        {
            SerialNumber = serialNumber,
            CompanyCode = companyCode,
            EmployeeSysId = empSysId,
            ItemCode = itemCode,
            EmployeeType = 'R',
            SwipeDate = DateTime.UtcNow.ToString("yyyy-MM-dd"),
            ItemType = 'M',
            EmployeeContribution = employeeContribution,
            EmployerContribution = employerContribution,
            CanteenNumber = canteenNumber,
            ItemQuantity = itemQuantity,
            EntryUser = entryUser,
            EntryDate = DateTime.UtcNow.ToString("yyyy-MM-dd")
        };

        availed.RaiseDomainEvent(new CanteenItemAvailedEvent(
            empSysId, itemCode, employeeContribution, employerContribution, DateTime.UtcNow));

        return availed;
    }
}
