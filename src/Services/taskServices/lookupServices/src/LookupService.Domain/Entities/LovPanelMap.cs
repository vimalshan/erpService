using LookupService.Domain.Common;

namespace LookupService.Domain.Entities;

public class LovPanelMap : BaseEntity
{
    public long? LpLovId { get; private set; }
    public decimal? LpPanelId { get; private set; }
    public string? LpFlag { get; private set; }

    // Navigation
    public LovMaster? LovMaster { get; private set; }
    public PanelMaster? PanelMaster { get; private set; }

    private LovPanelMap() { }

    public static LovPanelMap Create(long lovId, decimal panelId, string flag = "Y")
    {
        return new LovPanelMap
        {
            LpLovId = lovId,
            LpPanelId = panelId,
            LpFlag = flag
        };
    }

    public void SetFlag(string flag)
    {
        LpFlag = flag;
    }
}
