namespace ProblemManagement.Domain.Enums;

public enum ProblemCategory
{
    Function = '1',
    General = '2'
}

public static class ProblemCategoryExtensions
{
    public static char ToChar(this ProblemCategory category) => (char)category;

    public static ProblemCategory FromChar(char c) => c switch
    {
        '1' => ProblemCategory.Function,
        '2' => ProblemCategory.General,
        _ => throw new ArgumentOutOfRangeException(nameof(c), $"Invalid category: {c}")
    };
}
