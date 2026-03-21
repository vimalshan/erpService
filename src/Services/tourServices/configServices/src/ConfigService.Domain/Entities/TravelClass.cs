using ConfigService.Domain.Common;

namespace ConfigService.Domain.Entities;

public class TravelClass : AggregateRoot<string>
{
    public string ModeId { get; private set; } = string.Empty;
    public string ClassName { get; private set; } = string.Empty;
    public string ClassOrder { get; private set; } = string.Empty;

    private TravelClass() { }

    public static TravelClass Create(string id, string modeId, string name, string order)
    {
        return new TravelClass { Id = id, ModeId = modeId, ClassName = name, ClassOrder = order };
    }

    public void Update(string modeId, string name, string order)
    {
        ModeId = modeId;
        ClassName = name;
        ClassOrder = order;
    }
}
