using FinanceService.Domain.Common;

namespace FinanceService.Domain.ValueObjects;

public class InvoiceNumber : ValueObject
{
    public string Value { get; private set; }

    private InvoiceNumber() { Value = string.Empty; }

    public InvoiceNumber(string value)
    {
        if (string.IsNullOrWhiteSpace(value))
            throw new ArgumentException("Invoice number cannot be empty.", nameof(value));
        if (value.Length > 50)
            throw new ArgumentException("Invoice number cannot exceed 50 characters.", nameof(value));
        Value = value;
    }

    public static implicit operator string(InvoiceNumber invoiceNumber) => invoiceNumber.Value;

    protected override IEnumerable<object?> GetEqualityComponents()
    {
        yield return Value;
    }
}
