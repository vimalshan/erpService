namespace DocumentService.Domain.ValueObjects;

public record DocumentTypeId
{
    public long Value { get; }

    private DocumentTypeId(long value) => Value = value;

    public static DocumentTypeId From(long value)
    {
        if (value <= 0)
            throw new ArgumentException("DocumentTypeId must be a positive number.", nameof(value));
        return new DocumentTypeId(value);
    }

    public override string ToString() => Value.ToString();
}
