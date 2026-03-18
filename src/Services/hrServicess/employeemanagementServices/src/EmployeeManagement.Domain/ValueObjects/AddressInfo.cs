namespace EmployeeManagement.Domain.ValueObjects;

public sealed class AddressInfo
{
    public string? Line1 { get; }
    public string? Line2 { get; }
    public string? Line3 { get; }
    public string? Line4 { get; }
    public long? CityId { get; }
    public string? CityOthers { get; }
    public long? PinCode { get; }
    public long? StateId { get; }

    public AddressInfo(string? line1, string? line2, string? line3, string? line4,
        long? cityId, string? cityOthers, long? pinCode, long? stateId)
    {
        Line1 = line1;
        Line2 = line2;
        Line3 = line3;
        Line4 = line4;
        CityId = cityId;
        CityOthers = cityOthers;
        PinCode = pinCode;
        StateId = stateId;
    }
}
