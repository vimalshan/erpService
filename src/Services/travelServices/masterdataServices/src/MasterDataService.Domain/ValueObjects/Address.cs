using MasterDataService.Domain.Common;

namespace MasterDataService.Domain.ValueObjects;

public class Address : ValueObject
{
    public string City { get; private set; } = string.Empty;
    public string? State { get; private set; }
    public string? Country { get; private set; }

    private Address() { }

    public Address(string city, string? state = null, string? country = null)
    {
        City = city ?? throw new ArgumentNullException(nameof(city));
        State = state;
        Country = country;
    }

    protected override IEnumerable<object?> GetEqualityComponents()
    {
        yield return City;
        yield return State;
        yield return Country;
    }
}
