namespace VisitorServices.Domain.ValueObjects;

public sealed class ContactInfo
{
    public string? PhoneNumber { get; }
    public string? Email { get; }

    private ContactInfo() { }

    public ContactInfo(string? phoneNumber, string? email)
    {
        if (email != null && !email.Contains('@'))
            throw new ArgumentException("Invalid email address.", nameof(email));

        PhoneNumber = phoneNumber?.Trim();
        Email = email?.Trim().ToLowerInvariant();
    }

    public override bool Equals(object? obj) =>
        obj is ContactInfo other && PhoneNumber == other.PhoneNumber && Email == other.Email;

    public override int GetHashCode() => HashCode.Combine(PhoneNumber, Email);
}
