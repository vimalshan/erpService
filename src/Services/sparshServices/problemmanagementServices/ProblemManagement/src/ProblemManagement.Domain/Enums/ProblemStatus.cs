namespace ProblemManagement.Domain.Enums;

public enum ProblemStatus
{
    Posted = 'P',
    Accepted = 'A',
    Rejected = 'R'
}

public static class ProblemStatusExtensions
{
    public static char ToChar(this ProblemStatus status) => (char)status;

    public static ProblemStatus FromChar(char c) => c switch
    {
        'P' => ProblemStatus.Posted,
        'A' => ProblemStatus.Accepted,
        'R' => ProblemStatus.Rejected,
        _ => throw new ArgumentOutOfRangeException(nameof(c), $"Invalid problem status: {c}")
    };
}
