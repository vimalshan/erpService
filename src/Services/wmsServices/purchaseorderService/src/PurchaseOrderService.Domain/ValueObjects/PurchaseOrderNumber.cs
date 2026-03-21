namespace PurchaseOrderService.Domain.ValueObjects;

public record PurchaseOrderNumber
{
    public string Value { get; }

    public PurchaseOrderNumber(string value)
    {
        if (string.IsNullOrWhiteSpace(value))
            throw new ArgumentException("Purchase order number cannot be empty.", nameof(value));

        if (value.Length > 50)
            throw new ArgumentException("Purchase order number cannot exceed 50 characters.", nameof(value));

        Value = value;
    }

    public static implicit operator string(PurchaseOrderNumber poNumber) => poNumber.Value;
    public static explicit operator PurchaseOrderNumber(string value) => new(value);
    public override string ToString() => Value;
}
