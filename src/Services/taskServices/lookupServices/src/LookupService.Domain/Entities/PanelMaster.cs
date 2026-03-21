using LookupService.Domain.Common;

namespace LookupService.Domain.Entities;

public class PanelMaster : BaseEntity
{
    public decimal PanelId { get; private set; }
    public string? PanelName { get; private set; }

    // Navigation
    public ICollection<LovPanelMap> PanelMappings { get; private set; } = [];

    private PanelMaster() { }

    public static PanelMaster Create(decimal panelId, string panelName)
    {
        return new PanelMaster
        {
            PanelId = panelId,
            PanelName = panelName
        };
    }

    public void UpdateName(string name)
    {
        PanelName = name;
    }
}
