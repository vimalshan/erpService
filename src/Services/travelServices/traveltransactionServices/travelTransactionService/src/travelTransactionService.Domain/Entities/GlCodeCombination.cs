using travelTransactionService.Domain.Common;

namespace travelTransactionService.Domain.Entities;

public class GlCodeCombination : BaseEntity
{
    public long RowId { get; private set; }
    public long CodeCombinationId { get; private set; }
    public long ChartOfAccountsId { get; private set; }
    public string? ConcatenatedSegments { get; private set; }
    public string? PaddedConcatenatedSegments { get; private set; }
    public string GlAccountType { get; private set; } = null!;
    public string DetailBudgetingAllowed { get; private set; } = null!;
    public string DetailPostingAllowed { get; private set; } = null!;
    public string EnabledFlag { get; private set; } = null!;
    public string SummaryFlag { get; private set; } = null!;
    public string? Segment1 { get; private set; }
    public string? Segment2 { get; private set; }
    public string? Segment3 { get; private set; }
    public string? Segment4 { get; private set; }
    public string? Segment5 { get; private set; }
    public string? Segment6 { get; private set; }
    public string? Segment7 { get; private set; }
    public string? Description { get; private set; }
    public DateTime? StartDateActive { get; private set; }
    public DateTime? EndDateActive { get; private set; }
    public DateTime LastUpdateDate { get; private set; }
    public decimal LastUpdatedBy { get; private set; }

    private GlCodeCombination() { }

    public static GlCodeCombination Create(
        long rowId,
        long codeCombinationId,
        long chartOfAccountsId,
        string? concatenatedSegments,
        string glAccountType,
        string enabledFlag)
    {
        return new GlCodeCombination
        {
            RowId = rowId,
            CodeCombinationId = codeCombinationId,
            ChartOfAccountsId = chartOfAccountsId,
            ConcatenatedSegments = concatenatedSegments,
            GlAccountType = glAccountType,
            DetailBudgetingAllowed = "N",
            DetailPostingAllowed = "Y",
            EnabledFlag = enabledFlag,
            SummaryFlag = "N",
            LastUpdateDate = DateTime.UtcNow,
            LastUpdatedBy = 0
        };
    }
}
