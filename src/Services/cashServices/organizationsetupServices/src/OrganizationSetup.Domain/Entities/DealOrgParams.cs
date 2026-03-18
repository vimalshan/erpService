using OrganizationSetup.Domain.Common;
using OrganizationSetup.Domain.Events;
using OrganizationSetup.Domain.ValueObjects;

namespace OrganizationSetup.Domain.Entities;

/// <summary>Maps to DEAL_ORGPARAMS table - Organization Configuration Parameters.</summary>
public class DealOrgParams : BaseEntity
{
    public long OrgParamId { get; private set; }
    public ParameterType OrgParamType { get; private set; } = default!;
    public long OrgParamValue { get; private set; }
    public long OrgId { get; private set; }
    public decimal OrgModifiedBy { get; private set; }
    public DateTime OrgModifiedOn { get; private set; }

    private DealOrgParams() { }

    public static DealOrgParams Create(long paramId, string paramType, long paramValue, long orgId, decimal modifiedBy)
    {
        var param = new DealOrgParams
        {
            OrgParamId = paramId,
            OrgParamType = ParameterType.Create(paramType),
            OrgParamValue = paramValue,
            OrgId = orgId,
            OrgModifiedBy = modifiedBy,
            OrgModifiedOn = DateTime.UtcNow
        };
        param.AddDomainEvent(new OrgParamUpdatedEvent(paramId, paramType, paramValue, orgId));
        return param;
    }

    public void UpdateValue(long newValue, decimal modifiedBy)
    {
        OrgParamValue = newValue;
        OrgModifiedBy = modifiedBy;
        OrgModifiedOn = DateTime.UtcNow;
        AddDomainEvent(new OrgParamUpdatedEvent(OrgParamId, OrgParamType.Value, newValue, OrgId));
    }
}
