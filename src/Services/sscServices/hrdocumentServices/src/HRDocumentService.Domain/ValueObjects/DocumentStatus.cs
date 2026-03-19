namespace HRDocumentService.Domain.ValueObjects;

public sealed record DocumentStatus
{
    public string Value { get; }

    public static readonly DocumentStatus Draft = new("DR");
    public static readonly DocumentStatus Submitted = new("SB");
    public static readonly DocumentStatus Approved = new("AP");
    public static readonly DocumentStatus Rejected = new("RJ");
    public static readonly DocumentStatus Cancelled = new("CN");
    public static readonly DocumentStatus Paid = new("PD");

    private static readonly Dictionary<string, DocumentStatus> All = new()
    {
        ["DR"] = Draft,
        ["SB"] = Submitted,
        ["AP"] = Approved,
        ["RJ"] = Rejected,
        ["CN"] = Cancelled,
        ["PD"] = Paid
    };

    private DocumentStatus(string value) => Value = value;

    public static DocumentStatus From(string value)
    {
        var trimmed = value?.Trim().ToUpperInvariant()
            ?? throw new ArgumentNullException(nameof(value));

        if (!All.TryGetValue(trimmed, out var status))
            throw new ArgumentException($"Invalid document status: {trimmed}");

        return status;
    }

    public override string ToString() => Value;
}
