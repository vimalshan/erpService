namespace CustomerService.Domain.ValueObjects;

public sealed record ContactInfo
{
    public string ContactPerson { get; init; } = string.Empty;
    public string ContactTitle { get; init; } = string.Empty;
    public string Email { get; init; } = string.Empty;
    public string Phone { get; init; } = string.Empty;

    public ContactInfo() { }

    public ContactInfo(string contactPerson, string contactTitle, string email, string phone)
    {
        ContactPerson = contactPerson;
        ContactTitle = contactTitle;
        Email = email;
        Phone = phone;
    }
}
