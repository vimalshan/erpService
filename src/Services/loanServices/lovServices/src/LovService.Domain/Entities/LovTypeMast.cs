using LovService.Domain.Common;
using LovService.Domain.Events;
using LovService.Domain.ValueObjects;

namespace LovService.Domain.Entities;

/// <summary>
/// LOV_TYPEMAST - List of Values Type Master
/// </summary>
public class LovTypeMast : BaseEntity
{
    public int LovTypeId { get; private set; }
    public string LovTypeName { get; private set; } = string.Empty;
    public LovCategory LovCategory { get; private set; } = LovCategory.Fixed;
    public int LovOrgId { get; private set; }

    public ICollection<LovMaster> LovMasters { get; private set; } = [];

    private LovTypeMast() { }

    public static LovTypeMast Create(int lovTypeId, string lovTypeName, char lovCategory, int lovOrgId)
    {
        var entity = new LovTypeMast
        {
            LovTypeId = lovTypeId,
            LovTypeName = lovTypeName,
            LovCategory = LovCategory.From(lovCategory),
            LovOrgId = lovOrgId
        };
        entity.AddDomainEvent(new LovTypeCreatedEvent(entity));
        return entity;
    }

    public void Update(string lovTypeName, char lovCategory, int lovOrgId)
    {
        LovTypeName = lovTypeName;
        LovCategory = LovCategory.From(lovCategory);
        LovOrgId = lovOrgId;
        AddDomainEvent(new LovTypeUpdatedEvent(this));
    }
}
