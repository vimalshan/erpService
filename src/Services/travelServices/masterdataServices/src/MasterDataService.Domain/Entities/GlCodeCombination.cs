using MasterDataService.Domain.Common;
using MasterDataService.Domain.ValueObjects;

namespace MasterDataService.Domain.Entities;

public class GlCodeCombination : AuditableEntity
{
    public long RowId { get; private set; }
    public long CodeCombinationId { get; private set; }
    public long ChartOfAccountsId { get; private set; }
    public GlSegments Segments { get; private set; } = null!;
    public bool EnabledFlag { get; private set; }
    public bool SummaryFlag { get; private set; }
    public string? Context { get; private set; }
    public long LastUpdatedBy { get; private set; }
    public DateTime LastUpdateDate { get; private set; }

    private GlCodeCombination() { }

    public GlCodeCombination(long rowId, long codeCombinationId, long chartOfAccountsId,
        string concatenatedSegments, string accountType, bool enabled, bool summary,
        string? context, long lastUpdatedBy)
    {
        RowId = rowId;
        CodeCombinationId = codeCombinationId;
        ChartOfAccountsId = chartOfAccountsId;
        Segments = new GlSegments(concatenatedSegments, accountType);
        EnabledFlag = enabled;
        SummaryFlag = summary;
        Context = context;
        LastUpdatedBy = lastUpdatedBy;
        LastUpdateDate = DateTime.UtcNow;
    }

    public void Enable() => EnabledFlag = true;
    public void Disable() => EnabledFlag = false;
}
