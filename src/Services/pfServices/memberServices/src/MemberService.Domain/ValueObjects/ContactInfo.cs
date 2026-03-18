namespace MemberService.Domain.ValueObjects;

public sealed class ContactInfo : IEquatable<ContactInfo>
{
    public string? PhoneNo { get; }
    public string? Email { get; }

    private ContactInfo(string? phoneNo, string? email)
    {
        PhoneNo = phoneNo;
        Email = email;
    }

    public static ContactInfo Create(string? phoneNo = null, string? email = null)
    {
        if (email is not null && !email.Contains('@'))
            throw new ArgumentException("Invalid email format.", nameof(email));
        return new ContactInfo(phoneNo, email);
    }

    public bool Equals(ContactInfo? other) => other is not null && PhoneNo == other.PhoneNo && Email == other.Email;
    public override bool Equals(object? obj) => obj is ContactInfo ci && Equals(ci);
    public override int GetHashCode() => HashCode.Combine(PhoneNo, Email);
}
