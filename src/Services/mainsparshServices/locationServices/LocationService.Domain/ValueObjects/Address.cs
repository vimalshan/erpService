using System;

namespace LocationService.Domain.ValueObjects
{
    /// <summary>
    /// Value Object representing a complete address
    /// </summary>
    public class Address : IEquatable<Address>
    {
        public string? StreetAddress { get; private set; }
        public string? City { get; private set; }
        public string? State { get; private set; }
        public string? PostalCode { get; private set; }
        public string? Country { get; private set; }

        public Address() { }

        public Address(string? streetAddress, string? city, string? state, string? postalCode, string? country)
        {
            StreetAddress = streetAddress;
            City = city;
            State = state;
            PostalCode = postalCode;
            Country = country;
        }

        public override bool Equals(object? obj)
        {
            return Equals(obj as Address);
        }

        public bool Equals(Address? other)
        {
            if (other is null) return false;
            return StreetAddress == other.StreetAddress
                && City == other.City
                && State == other.State
                && PostalCode == other.PostalCode
                && Country == other.Country;
        }

        public override int GetHashCode()
        {
            return HashCode.Combine(StreetAddress, City, State, PostalCode, Country);
        }

        public override string ToString()
        {
            return $"{StreetAddress}, {City}, {State} {PostalCode}, {Country}";
        }
    }
}
