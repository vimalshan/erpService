using DealTicketing.Domain.Common;

namespace DealTicketing.Domain.Entities;

/// <summary>
/// List-of-Values lookup.
/// LOV_TYPE: 001=DerivativeType, 002=DealNature, 003=DealCategory, 004=OptionsType, 005=FloatingBase
/// </summary>
public class LovMaster : BaseEntity
{
    public long LovId { get; private set; }
    public string LovType { get; private set; } = default!;
    public string LovName { get; private set; } = default!;

    private LovMaster() { }

    public LovMaster(long id, string type, string name)
    {
        LovId = id;
        LovType = type;
        LovName = name;
    }
}
