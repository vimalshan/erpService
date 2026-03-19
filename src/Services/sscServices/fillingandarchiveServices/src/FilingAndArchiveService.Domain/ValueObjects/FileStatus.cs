using FilingAndArchiveService.Domain.Common;

namespace FilingAndArchiveService.Domain.ValueObjects;

public sealed class FileStatus : ValueObject
{
    public static readonly FileStatus Active = new("A");
    public static readonly FileStatus Closed = new("C");
    public static readonly FileStatus Dispatched = new("D");
    public static readonly FileStatus Archived = new("R");

    private static readonly HashSet<string> ValidCodes = new(StringComparer.OrdinalIgnoreCase)
    {
        "A", "C", "D", "R"
    };

    public string Code { get; }

    private FileStatus(string code) => Code = code;

    public static FileStatus From(string code)
    {
        if (string.IsNullOrWhiteSpace(code))
            throw new ArgumentException("File status code cannot be empty.", nameof(code));

        var upper = code.ToUpperInvariant();
        if (!ValidCodes.Contains(upper))
            throw new ArgumentException($"'{code}' is not a valid file status code.", nameof(code));

        return new FileStatus(upper);
    }

    public override string ToString() => Code;

    protected override IEnumerable<object> GetEqualityComponents()
    {
        yield return Code;
    }
}
