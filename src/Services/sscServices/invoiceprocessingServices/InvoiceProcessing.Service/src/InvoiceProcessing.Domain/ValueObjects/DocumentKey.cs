namespace InvoiceProcessing.Domain.ValueObjects;

public record DocumentKey(string Value)
{
    public static DocumentKey Create(string value) => new(value);
    public override string ToString() => Value;
}
