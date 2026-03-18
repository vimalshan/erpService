namespace MemberService.Domain.ValueObjects;

public sealed class Address : IEquatable<Address>
{
    public string Line1 { get; }
    public string? Line2 { get; }
    public string? Line3 { get; }
    public string City { get; }
    public string State { get; }
    public string PinCode { get; }
    public string Country { get; }

    private Address(string line1, string? line2, string? line3, string city, string state, string pinCode, string country)
    {
        Line1 = line1;
        Line2 = line2;
        Line3 = line3;
        City = city;
        State = state;
        PinCode = pinCode;
        Country = country;
    }

    public static Address Create(string line1, string city, string state, string pinCode, string country,
        string? line2 = null, string? line3 = null)
    {
        if (string.IsNullOrWhiteSpace(line1)) throw new ArgumentException("Address line 1 is required.", nameof(line1));
        if (string.IsNullOrWhiteSpace(city)) throw new ArgumentException("City is required.", nameof(city));
        if (string.IsNullOrWhiteSpace(state)) throw new ArgumentException("State is required.", nameof(state));
        if (string.IsNullOrWhiteSpace(pinCode)) throw new ArgumentException("Pin code is required.", nameof(pinCode));
        if (string.IsNullOrWhiteSpace(country)) throw new ArgumentException("Country is required.", nameof(country));
        return new Address(line1, line2, line3, city, state, pinCode, country);
    }

    public bool Equals(Address? other) =>
        other is not null && Line1 == other.Line1 && City == other.City &&
        State == other.State && PinCode == other.PinCode && Country == other.Country;
    public override bool Equals(object? obj) => obj is Address a && Equals(a);
    public override int GetHashCode() => HashCode.Combine(Line1, City, State, PinCode, Country);
}
