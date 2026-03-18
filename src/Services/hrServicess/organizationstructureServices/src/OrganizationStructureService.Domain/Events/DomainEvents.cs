using OrganizationStructureService.Domain.Common;

namespace OrganizationStructureService.Domain.Events;

public sealed class BusinessCreatedEvent : DomainEvent
{
    public decimal BusinessId { get; }
    public string BusinessName { get; }

    public BusinessCreatedEvent(decimal businessId, string businessName)
    {
        BusinessId = businessId;
        BusinessName = businessName;
    }
}

public sealed class BusinessUpdatedEvent : DomainEvent
{
    public decimal BusinessId { get; }
    public string BusinessName { get; }

    public BusinessUpdatedEvent(decimal businessId, string businessName)
    {
        BusinessId = businessId;
        BusinessName = businessName;
    }
}

public sealed class UnitCreatedEvent : DomainEvent
{
    public decimal UnitId { get; }
    public string UnitName { get; }
    public decimal BusinessId { get; }

    public UnitCreatedEvent(decimal unitId, string unitName, decimal businessId)
    {
        UnitId = unitId;
        UnitName = unitName;
        BusinessId = businessId;
    }
}

public sealed class UnitUpdatedEvent : DomainEvent
{
    public decimal UnitId { get; }
    public string UnitName { get; }

    public UnitUpdatedEvent(decimal unitId, string unitName)
    {
        UnitId = unitId;
        UnitName = unitName;
    }
}

public sealed class GradeCreatedEvent : DomainEvent
{
    public decimal GradeId { get; }
    public string? GradeName { get; }

    public GradeCreatedEvent(decimal gradeId, string? gradeName)
    {
        GradeId = gradeId;
        GradeName = gradeName;
    }
}

public sealed class PositionCreatedEvent : DomainEvent
{
    public decimal PositionId { get; }
    public string Designation { get; }
    public decimal GradeId { get; }

    public PositionCreatedEvent(decimal positionId, string designation, decimal gradeId)
    {
        PositionId = positionId;
        Designation = designation;
        GradeId = gradeId;
    }
}
