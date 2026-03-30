using EmployeeTransactionsService.Domain.Common;

namespace EmployeeTransactionsService.Domain.Entities;

public sealed class AlertGroupEmployeeMap : BaseEntity
{
    private AlertGroupEmployeeMap()
    {
    }

    public decimal AlmapId { get; private set; }
    public decimal AlmapGrpid { get; private set; }
    public decimal AlmapEmpSysId { get; private set; }
    public string? AlmapEmailId { get; private set; }
    public decimal AlmapOrgId { get; private set; }
    public decimal AlmapUnitId { get; private set; }
    public decimal? AlmapCalendarId { get; private set; }
    public DateTime AlmapEffDate { get; private set; }
    public DateTime? AlmapClsDate { get; private set; }
    public decimal AlmapCreatedBy { get; private set; }
    public DateTime AlmapCreatedOn { get; private set; }
    public decimal? AlmapModifiedBy { get; private set; }
    public DateTime? AlmapModifiedOn { get; private set; }

    public static AlertGroupEmployeeMap Create(decimal id, decimal groupId, decimal empSysId, string? emailId, decimal orgId, decimal unitId, decimal? calendarId, DateTime effectiveDate, DateTime? closeDate, decimal createdBy)
    {
        return new AlertGroupEmployeeMap
        {
            AlmapId = id,
            AlmapGrpid = groupId,
            AlmapEmpSysId = empSysId,
            AlmapEmailId = emailId,
            AlmapOrgId = orgId,
            AlmapUnitId = unitId,
            AlmapCalendarId = calendarId,
            AlmapEffDate = effectiveDate,
            AlmapClsDate = closeDate,
            AlmapCreatedBy = createdBy,
            AlmapCreatedOn = DateTime.UtcNow
        };
    }
}