namespace WarehouseStructure.Domain.ValueObjects;

public sealed class Address : IEquatable<Address>
{
    public string? Street { get; }
    public string? City { get; }
    public string? State { get; }
    public string? Country { get; }
    public string? PostalCode { get; }

    public Address(string? street, string? city, string? state, string? country, string? postalCode)
    {
        Street = street;
        City = city;
        State = state;
        Country = country;
        PostalCode = postalCode;
    }

    public bool Equals(Address? other)
    {
        if (other is null) return false;
        return Street == other.Street
            && City == other.City
            && State == other.State
            && Country == other.Country
            && PostalCode == other.PostalCode;
    }

    public override bool Equals(object? obj) => Equals(obj as Address);

    public override int GetHashCode() => HashCode.Combine(Street, City, State, Country, PostalCode);
}
