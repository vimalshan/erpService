using SwipeTransactionService.Domain.Common;
using SwipeTransactionService.Domain.Events;

namespace SwipeTransactionService.Domain.Entities;

/// <summary>
/// Represents an employee's day-wise canteen punch record.
/// Maps to CAN_DAYWISE_EMP_PUNCH.
/// </summary>
public sealed class CanteenPunch : BaseEntity
{
    public long? SerialNumber { get; private set; }
    public long CompanyCode { get; private set; }
    public long EmployeeSysId { get; private set; }
    public long CanteenUnit { get; private set; }
    public DateTime PunchDate { get; private set; }
    public string? TimeIn { get; private set; }
    public string? TimeOut { get; private set; }
    public decimal? WorkHours { get; private set; }

    private CanteenPunch() { }

    public static CanteenPunch CreateCheckIn(long serialNumber, long companyCode, long empSysId, long canteenUnit, DateTime punchDateTime)
    {
        var punch = new CanteenPunch
        {
            SerialNumber = serialNumber,
            CompanyCode = companyCode,
            EmployeeSysId = empSysId,
            CanteenUnit = canteenUnit,
            PunchDate = punchDateTime.Date,
            TimeIn = punchDateTime.ToString("HH:mm:ss")
        };

        punch.RaiseDomainEvent(new EmployeePunchedInEvent(empSysId, canteenUnit, punchDateTime));
        return punch;
    }

    public void RecordCheckOut(DateTime checkOutTime)
    {
        TimeOut = checkOutTime.ToString("HH:mm:ss");

        if (TimeIn != null && TimeSpan.TryParse(TimeIn, out var timeIn))
        {
            var timeOut = checkOutTime.TimeOfDay;
            var duration = timeOut - timeIn;
            WorkHours = duration.TotalHours >= 0 ? (decimal)duration.TotalHours : 0;
        }

        RaiseDomainEvent(new EmployeePunchedOutEvent(EmployeeSysId, CanteenUnit, checkOutTime, WorkHours ?? 0));
    }
}
