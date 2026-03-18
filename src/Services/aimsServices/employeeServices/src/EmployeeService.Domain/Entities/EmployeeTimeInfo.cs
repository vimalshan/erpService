using EmployeeService.Domain.Common;
using EmployeeService.Domain.ValueObjects;

namespace EmployeeService.Domain.Entities;

/// <summary>Maps to EMP_TIMEINFO — Employee attendance flag record.</summary>
public sealed class EmployeeTimeInfo : BaseAuditableEntity
{
    public long TimeInfoId { get; private set; }
    public EmployeeId EmpSysId { get; private set; } = null!;
    public AttendanceFlag EmpAttFlag { get; private set; } = null!;

    private EmployeeTimeInfo() { }

    public static EmployeeTimeInfo Create(long timeInfoId, long empSysId, char attFlag, long modifiedBy)
    {
        var entity = new EmployeeTimeInfo
        {
            TimeInfoId = timeInfoId,
            EmpSysId = EmployeeId.Of(empSysId),
            EmpAttFlag = AttendanceFlag.Of(attFlag),
            LastModifiedBy = modifiedBy,
            LastModifiedOn = DateTime.UtcNow
        };
        entity.AddDomainEvent(new Events.TimeInfoUpdatedEvent(timeInfoId, empSysId, attFlag));
        return entity;
    }

    public void UpdateFlag(char newFlag, long modifiedBy)
    {
        EmpAttFlag = AttendanceFlag.Of(newFlag);
        LastModifiedBy = modifiedBy;
        LastModifiedOn = DateTime.UtcNow;
        AddDomainEvent(new Events.TimeInfoUpdatedEvent(TimeInfoId, EmpSysId.Value, newFlag));
    }
}
