namespace ConfigService.Domain.ValueObjects;

using ConfigService.Domain.Common;

public class Address : ValueObject
{
    public string Address1 { get; }
    public string Address2 { get; }
    public string Address3 { get; }
    public string Address4 { get; }
    public string PinCode { get; }

    public Address(string address1, string address2, string address3, string address4, string pinCode)
    {
        Address1 = address1;
        Address2 = address2;
        Address3 = address3;
        Address4 = address4;
        PinCode = pinCode;
    }

    protected override IEnumerable<object?> GetEqualityComponents()
    {
        yield return Address1;
        yield return Address2;
        yield return Address3;
        yield return Address4;
        yield return PinCode;
    }
}
