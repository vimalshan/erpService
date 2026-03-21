using LookupService.Domain.Common;
using LookupService.Domain.Events;

namespace LookupService.Domain.Entities;

public class UnitLovAccessMaster : AggregateRoot
{
    public decimal UaAccessMastId { get; private set; }
    public decimal? UaUnitLovMapId { get; private set; }
    public decimal? UaDepartmentId { get; private set; }
    public decimal? UaProcessId { get; private set; }

    // Navigation
    public LovUnitMap? UnitLovMap { get; private set; }
    public ProcessMaster? ProcessMaster { get; private set; }
    public ICollection<UnitLovAccessDetail> AccessDetails { get; private set; } = [];

    private UnitLovAccessMaster() { }

    public static UnitLovAccessMaster Create(decimal accessMastId, decimal unitLovMapId, decimal departmentId, decimal processId)
    {
        var entity = new UnitLovAccessMaster
        {
            UaAccessMastId = accessMastId,
            UaUnitLovMapId = unitLovMapId,
            UaDepartmentId = departmentId,
            UaProcessId = processId
        };

        entity.AddDomainEvent(new AccessMasterCreatedEvent(accessMastId, unitLovMapId));
        return entity;
    }
}
