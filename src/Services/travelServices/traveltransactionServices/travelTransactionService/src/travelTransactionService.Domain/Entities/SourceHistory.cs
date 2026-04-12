using travelTransactionService.Domain.Common;

namespace travelTransactionService.Domain.Entities;

public class SourceHistory : BaseEntity
{
    public DateTime? ChangeDate { get; private set; }
    public string? Name { get; private set; }
    public string? Type { get; private set; }
    public decimal? Line { get; private set; }
    public string? Text { get; private set; }

    private SourceHistory() { }

    public static SourceHistory Create(
        string? name,
        string? type,
        decimal? line,
        string? text)
    {
        return new SourceHistory
        {
            ChangeDate = DateTime.UtcNow,
            Name = name,
            Type = type,
            Line = line,
            Text = text
        };
    }
}
