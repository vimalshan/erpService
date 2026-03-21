using BookingService.Domain.Common;

namespace BookingService.Domain.ValueObjects;

public sealed class PersonName : ValueObject
{
    public string FullName { get; }

    private PersonName() => FullName = string.Empty;
    private PersonName(string fullName) => FullName = fullName;

    public static PersonName Create(string fullName)
    {
        if (string.IsNullOrWhiteSpace(fullName))
            throw new ArgumentException("Person name cannot be empty.");
        if (fullName.Length > 200)
            throw new ArgumentException("Person name cannot exceed 200 characters.");
        return new PersonName(fullName.Trim());
    }

    protected override IEnumerable<object?> GetEqualityComponents()
    {
        yield return FullName.ToUpperInvariant();
    }

    public override string ToString() => FullName;
}
