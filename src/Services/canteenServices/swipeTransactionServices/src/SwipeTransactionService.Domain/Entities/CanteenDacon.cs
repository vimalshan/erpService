using SwipeTransactionService.Domain.Common;
using SwipeTransactionService.Domain.Events;

namespace SwipeTransactionService.Domain.Entities;

/// <summary>
/// Canteen daily contribution record (DACON).
/// Maps to CANTEEDN_DACON.
/// </summary>
public sealed class CanteenDacon : BaseEntity
{
    public long? SerialNumber { get; private set; }
    public long? CompanyCode { get; private set; }
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

    private CanteenDacon() { }

    public static CanteenDacon Record(
        long serialNumber,
        long companyCode,
        long empSysId,
        long itemCode,
        decimal employeeShare,
        decimal employerShare,
        string canteenNumber)
    {
        var dacon = new CanteenDacon
        {
            SerialNumber = serialNumber,
            CompanyCode = companyCode,
            EmployeeSysId = empSysId,
            EmployeeType = 'R',
            SwipeDate = DateTime.UtcNow.ToString("yyyy-MM-dd"),
            ItemCode = itemCode,
            ItemType = 'M',
            EmployeeContribution = employeeShare,
            EmployerContribution = employerShare,
            CanteenNumber = canteenNumber,
            EntryDate = DateTime.UtcNow.ToString("yyyy-MM-dd")
        };

        dacon.RaiseDomainEvent(new CanteenTransactionRecordedEvent(
            empSysId, itemCode, employeeShare, employerShare));

        return dacon;
    }
}
