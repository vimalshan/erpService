using UserManagement.Domain.Common;

namespace UserManagement.Domain.ValueObjects;

public sealed class PhoneNumber : IEquatable<PhoneNumber>
{
    public string Value { get; }

    private PhoneNumber(string value) => Value = value;

    public static PhoneNumber Create(string phone)
    {
        if (string.IsNullOrWhiteSpace(phone))
            throw new DomainException("Phone number cannot be empty.");

        var cleaned = System.Text.RegularExpressions.Regex.Replace(phone, @"[\s\-\(\)\+]", "");

        if (cleaned.Length < 6 || cleaned.Length > 20 || !cleaned.All(char.IsDigit))
            throw new DomainException($"'{phone}' is not a valid phone number.");

        return new PhoneNumber(phone.Trim());
    }

    public bool Equals(PhoneNumber? other) => other is not null && Value == other.Value;
    public override bool Equals(object? obj) => obj is PhoneNumber p && Equals(p);
    public override int GetHashCode() => Value.GetHashCode();
    public override string ToString() => Value;
}
