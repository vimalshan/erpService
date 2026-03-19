namespace HRDocumentService.Domain.ValueObjects;

public sealed record DocumentSource
{
    public string Value { get; }

    private static readonly HashSet<string> ValidSources = ["SSC", "HRD", "EMP", "SYS"];

    private DocumentSource(string value) => Value = value;

    public static DocumentSource Create(string value)
    {
        var trimmed = value?.Trim().ToUpperInvariant()
            ?? throw new ArgumentNullException(nameof(value));

        if (trimmed.Length != 3)
            throw new ArgumentException("Document source must be 3 characters.", nameof(value));

        if (!ValidSources.Contains(trimmed))
            throw new ArgumentException($"Invalid document source: {trimmed}. Valid sources: {string.Join(", ", ValidSources)}", nameof(value));

        return new DocumentSource(trimmed);
    }

    public override string ToString() => Value;
}
