using MasterDataService.Domain.Common;
using MasterDataService.Domain.Events;

namespace MasterDataService.Domain.Entities;

public class Configuration : AggregateRoot
{
    public int ConfigId { get; set; }
    public string ConfigKey { get; set; } = string.Empty;
    public string ConfigValue { get; set; } = string.Empty;
    public string ConfigType { get; set; } = string.Empty;
    public string? ConfigDescription { get; set; }
    public DateTime CreatedDate { get; set; }
    public DateTime? UpdatedDate { get; set; }
    public long CreatedBy { get; set; }

    public void UpdateValue(string newValue)
    {
        var oldValue = ConfigValue;
        ConfigValue = newValue;
        UpdatedDate = DateTime.UtcNow;
        AddDomainEvent(new ConfigurationChangedEvent(ConfigKey, oldValue, newValue));
    }
}
