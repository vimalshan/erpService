using ArchiveService.Domain.Common;

namespace ArchiveService.Domain.ValueObjects;

public class Address : ValueObject
{
    public string FullAddress { get; private set; } = string.Empty;

    private Address() { }

    public Address(string fullAddress)
    {
        FullAddress = fullAddress ?? string.Empty;
    }

    protected override IEnumerable<object?> GetEqualityComponents()
    {
        yield return FullAddress;
    }
}
