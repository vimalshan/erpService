using MasterDataService.Domain.Common;

namespace MasterDataService.Domain.Events;

public sealed class ConfigurationChangedEvent : IDomainEvent
{
    public string ConfigKey { get; }
    public string OldValue { get; }
    public string NewValue { get; }
    public DateTime OccurredOn { get; } = DateTime.UtcNow;

    public ConfigurationChangedEvent(string configKey, string oldValue, string newValue)
    {
        ConfigKey = configKey;
        OldValue = oldValue;
        NewValue = newValue;
    }
}
