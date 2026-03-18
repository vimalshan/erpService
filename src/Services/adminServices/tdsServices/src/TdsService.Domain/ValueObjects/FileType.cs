using TdsService.Domain.Exceptions;

namespace TdsService.Domain.ValueObjects;

/// <summary>
/// Represents the type of a TDS file (e.g. 26A, 16A, 16B, 27Q etc.).
/// Stored as VARCHAR(3).
/// </summary>
public sealed class FileType
{
    private static readonly HashSet<string> KnownTypes =
    [
        "26A", "16A", "16B", "27Q", "27A", "16", "12B"
    ];

    public string Value { get; }

    private FileType(string value) => Value = value;

    public static FileType Create(string? type)
    {
        if (string.IsNullOrWhiteSpace(type))
            throw new DomainException("File type cannot be empty.");

        var normalised = type.Trim().ToUpperInvariant();

        if (normalised.Length > 3)
            throw new DomainException($"File type '{type}' exceeds maximum length of 3 characters.");

        return new FileType(normalised);
    }

    public static bool IsKnownType(string type) => KnownTypes.Contains(type.ToUpperInvariant());

    public override string ToString() => Value;
    public override bool Equals(object? obj) => obj is FileType ft && ft.Value == Value;
    public override int GetHashCode() => Value.GetHashCode();
}
