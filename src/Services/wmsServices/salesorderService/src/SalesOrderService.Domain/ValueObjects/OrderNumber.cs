namespace SalesOrderService.Domain.ValueObjects;

/// <summary>
/// Unique identifier for a Sales Order — wraps the business-friendly SO number.
/// </summary>
public sealed record OrderNumber(string Value)
{
    public static OrderNumber From(string value)
    {
        ArgumentException.ThrowIfNullOrWhiteSpace(value);
        if (value.Length > 50)
            throw new ArgumentException("Order number cannot exceed 50 characters.");
        return new OrderNumber(value.Trim().ToUpperInvariant());
    }

    public override string ToString() => Value;
}
