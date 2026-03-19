namespace HRDocumentService.Domain.ValueObjects;

public sealed record DocumentType
{
    public string Value { get; }

    private static readonly HashSet<string> ValidTypes = ["PAY", "TAX", "INS", "BEN", "LVE", "OTH"];

    private DocumentType(string value) => Value = value;

    public static DocumentType Create(string value)
    {
        var trimmed = value?.Trim().ToUpperInvariant()
            ?? throw new ArgumentNullException(nameof(value));

        if (trimmed.Length != 3)
            throw new ArgumentException("Document type must be 3 characters.", nameof(value));

        if (!ValidTypes.Contains(trimmed))
            throw new ArgumentException($"Invalid document type: {trimmed}. Valid types: {string.Join(", ", ValidTypes)}", nameof(value));

        return new DocumentType(trimmed);
    }

    public override string ToString() => Value;
}
