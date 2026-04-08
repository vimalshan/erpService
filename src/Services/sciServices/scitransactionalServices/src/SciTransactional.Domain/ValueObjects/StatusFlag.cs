using SciTransactional.Domain.Common;

namespace SciTransactional.Domain.ValueObjects;

public sealed class StatusFlag : ValueObject
{
    public string Code { get; }

    private StatusFlag(string code) => Code = code;

    public static readonly StatusFlag Active = new("A");
    public static readonly StatusFlag Closed = new("C");
    public static readonly StatusFlag New = new("N");
    public static readonly StatusFlag Pending = new("P");

    private static readonly Dictionary<string, StatusFlag> ValidFlags = new()
    {
        ["A"] = Active,
        ["C"] = Closed,
        ["N"] = New,
        ["P"] = Pending
    };

    public static StatusFlag FromCode(string code)
    {
        if (string.IsNullOrWhiteSpace(code))
            throw new ArgumentException("Status code is required.");
        var upper = code.Trim().ToUpperInvariant();
        if (!ValidFlags.TryGetValue(upper, out var flag))
            throw new ArgumentException($"Invalid status code '{code}'.");
        return flag;
    }

    protected override IEnumerable<object?> GetEqualityComponents()
    {
        yield return Code;
    }
}
