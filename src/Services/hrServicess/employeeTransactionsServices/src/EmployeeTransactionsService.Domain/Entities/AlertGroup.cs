using EmployeeTransactionsService.Domain.Common;
using EmployeeTransactionsService.Domain.Events;

namespace EmployeeTransactionsService.Domain.Entities;

public sealed class AlertGroup : BaseEntity
{
    private AlertGroup()
    {
    }

    public decimal AlgrpId { get; private set; }
    public string AlgrpName { get; private set; } = string.Empty;
    public string AlgrpType { get; private set; } = "R";
    public decimal AlgrpCreatedBy { get; private set; }
    public DateTime AlgrpCreatedOn { get; private set; }
    public decimal? AlgrpModifiedBy { get; private set; }
    public DateTime? AlgrpModifiedOn { get; private set; }

    public List<AlertGroupEmployeeMap> Members { get; private set; } = [];

    public static AlertGroup Create(decimal id, string name, string type, decimal createdBy)
    {
        var entity = new AlertGroup
        {
            AlgrpId = id,
            AlgrpName = name.Trim(),
            AlgrpType = type[..1].ToUpperInvariant(),
            AlgrpCreatedBy = createdBy,
            AlgrpCreatedOn = DateTime.UtcNow
        };

        entity.AddDomainEvent(new AlertGroupCreatedDomainEvent(id, entity.AlgrpName));
        return entity;
    }

    public void AddRecipient(decimal mapId, decimal? empSysId, string? emailId, decimal orgId, decimal unitId, decimal? calendarId, DateTime effectiveDate, DateTime? closeDate, decimal createdBy)
    {
        Members.Add(AlertGroupEmployeeMap.Create(
            mapId,
            AlgrpId,
            empSysId ?? 0,
            emailId,
            orgId,
            unitId,
            calendarId,
            effectiveDate,
            closeDate,
            createdBy));
    }
}