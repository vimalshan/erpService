using LovService.Domain.Common;
using LovService.Domain.Events;

namespace LovService.Domain.Entities;

/// <summary>
/// PROGRAMLOV_MAST - Program LOV Master (composite primary key)
/// </summary>
public class ProgramLovMast : BaseEntity
{
    public string PrlovTypeCode { get; private set; } = string.Empty;
    public string PrlovCode { get; private set; } = string.Empty;
    public string PrlovName { get; private set; } = string.Empty;

    private ProgramLovMast() { }

    public static ProgramLovMast Create(string prlovTypeCode, string prlovCode, string prlovName)
    {
        var entity = new ProgramLovMast
        {
            PrlovTypeCode = prlovTypeCode,
            PrlovCode = prlovCode,
            PrlovName = prlovName
        };
        entity.AddDomainEvent(new ProgramLovCreatedEvent(entity));
        return entity;
    }

    public void Update(string prlovName)
    {
        PrlovName = prlovName;
        AddDomainEvent(new ProgramLovUpdatedEvent(this));
    }
}
