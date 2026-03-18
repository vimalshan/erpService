namespace TrustService.Domain.ValueObjects;

public record Address
{
    public string AddressLine1 { get; init; } = string.Empty;
    public string? AddressLine2 { get; init; }
    public string? AddressLine3 { get; init; }
    public string? City { get; init; }
    public string? State { get; init; }
    public string? PinCode { get; init; }
    public string? Country { get; init; }

    public static Address Create(string line1, string? line2 = null, string? line3 = null,
        string? city = null, string? state = null, string? pinCode = null, string? country = null)
    {
        if (string.IsNullOrWhiteSpace(line1))
            throw new ArgumentException("Address line 1 is required.", nameof(line1));

        return new Address
        {
            AddressLine1 = line1,
            AddressLine2 = line2,
            AddressLine3 = line3,
            City = city,
            State = state,
            PinCode = pinCode,
            Country = country
        };
    }
}
