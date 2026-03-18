using TrustService.Domain.Common;

namespace TrustService.Domain.Entities;

public class TrustConfiguration : BaseEntity
{
    public long ConfigId { get; private set; }
    public string TrustCode { get; private set; } = string.Empty;
    public string ConfigName { get; private set; } = string.Empty;
    public string ConfigValue { get; private set; } = string.Empty;
    public string ConfigCategory { get; private set; } = string.Empty;
    public DateTime EffDate { get; private set; }
    public DateTime? ClsDate { get; private set; }

    public TrustMaster Trust { get; private set; } = null!;

    private TrustConfiguration() { }

    public static TrustConfiguration Create(string trustCode, string configName, string configValue,
        string configCategory, DateTime effDate)
    {
        return new TrustConfiguration
        {
            TrustCode = trustCode,
            ConfigName = configName,
            ConfigValue = configValue,
            ConfigCategory = configCategory,
            EffDate = effDate
        };
    }

    public void UpdateValue(string newValue)
    {
        ConfigValue = newValue;
    }

    public void Close(DateTime closureDate)
    {
        ClsDate = closureDate;
    }
}
