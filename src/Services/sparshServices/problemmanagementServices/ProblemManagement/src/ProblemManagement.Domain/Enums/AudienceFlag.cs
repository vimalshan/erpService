namespace ProblemManagement.Domain.Enums;

public enum AudienceFlag
{
    All = '0',
    Selected = '1'
}

public static class AudienceFlagExtensions
{
    public static char ToChar(this AudienceFlag flag) => (char)flag;

    public static AudienceFlag FromChar(char c) => c switch
    {
        '0' => AudienceFlag.All,
        '1' => AudienceFlag.Selected,
        _ => throw new ArgumentOutOfRangeException(nameof(c), $"Invalid audience flag: {c}")
    };
}
