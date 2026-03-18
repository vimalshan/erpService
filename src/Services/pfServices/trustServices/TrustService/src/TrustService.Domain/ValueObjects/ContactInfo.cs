namespace TrustService.Domain.ValueObjects;

public record ContactInfo
{
    public string? PhoneNo { get; init; }
    public string? FaxNo { get; init; }
    public string? Email { get; init; }

    public static ContactInfo Create(string? phone = null, string? fax = null, string? email = null)
    {
        return new ContactInfo
        {
            PhoneNo = phone,
            FaxNo = fax,
            Email = email
        };
    }
}
