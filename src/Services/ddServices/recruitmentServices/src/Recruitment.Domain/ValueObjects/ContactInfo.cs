namespace Recruitment.Domain.ValueObjects;

public class ContactInfo
{
    public string SparshId { get; private set; }
    public decimal SparshPin { get; private set; }

    public ContactInfo(string sparshId, decimal sparshPin)
    {
        if (string.IsNullOrWhiteSpace(sparshId))
            throw new ArgumentException("Sparsh ID cannot be empty", nameof(sparshId));
        if (sparshPin <= 0)
            throw new ArgumentException("Sparsh PIN must be positive", nameof(sparshPin));

        SparshId = sparshId;
        SparshPin = sparshPin;
    }

    public override bool Equals(object obj)
    {
        if (obj is not ContactInfo other)
            return false;
        return SparshId == other.SparshId && SparshPin == other.SparshPin;
    }

    public override int GetHashCode()
    {
        return HashCode.Combine(SparshId, SparshPin);
    }
}
