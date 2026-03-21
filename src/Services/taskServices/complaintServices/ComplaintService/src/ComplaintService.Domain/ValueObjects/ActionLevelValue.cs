using ComplaintService.Domain.Enums;

namespace ComplaintService.Domain.ValueObjects;

public sealed record ActionLevelValue
{
    public ActionLevel Level { get; }

    public ActionLevelValue(ActionLevel level) => Level = level;

    public char ToChar() => Level switch
    {
        ActionLevel.Primary => 'P',
        ActionLevel.Secondary => 'S',
        ActionLevel.Forward => 'F',
        ActionLevel.Corrective => 'C',
        _ => throw new ArgumentOutOfRangeException()
    };

    public static ActionLevelValue FromChar(char c) => c switch
    {
        'P' => new ActionLevelValue(ActionLevel.Primary),
        'S' => new ActionLevelValue(ActionLevel.Secondary),
        'F' => new ActionLevelValue(ActionLevel.Forward),
        'C' => new ActionLevelValue(ActionLevel.Corrective),
        _ => throw new ArgumentException($"Invalid action level char: {c}")
    };
}
