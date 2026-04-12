using travelTransactionService.Domain.Common;

namespace travelTransactionService.Domain.Entities;

public class JvInterface : BaseEntity
{
    public decimal? CodeCombination { get; private set; }
    public string? Segment1 { get; private set; }
    public decimal? Io { get; private set; }
    public string? Unit { get; private set; }

    private JvInterface() { }

    public static JvInterface Create(decimal? codeCombination, string? segment1, decimal? io, string? unit)
    {
        return new JvInterface
        {
            CodeCombination = codeCombination,
            Segment1 = segment1,
            Io = io,
            Unit = unit
        };
    }
}
