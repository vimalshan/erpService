namespace ComplaintService.Domain.ValueObjects;

public sealed record TicketNumber
{
    public decimal Value { get; }

    public TicketNumber(decimal value)
    {
        if (value <= 0)
            throw new ArgumentException("Ticket number must be positive.", nameof(value));
        Value = value;
    }

    public static implicit operator decimal(TicketNumber t) => t.Value;
    public override string ToString() => Value.ToString();
}
