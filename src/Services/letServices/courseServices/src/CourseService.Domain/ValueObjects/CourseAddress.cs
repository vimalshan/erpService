using CourseService.Domain.Common;

namespace CourseService.Domain.ValueObjects;

/// <summary>
/// Represents the physical address of a course venue.
/// </summary>
public sealed record CourseAddress
{
    public char LocationCode { get; }
    public string AddressLine1 { get; }
    public string AddressLine2 { get; }
    public string AddressLine3 { get; }
    public long PinCode { get; }
    public string PhoneNumber { get; }

    private CourseAddress()
    {
        AddressLine1 = string.Empty;
        AddressLine2 = string.Empty;
        AddressLine3 = string.Empty;
        PhoneNumber = string.Empty;
    }

    public CourseAddress(char locationCode, string addressLine1, string addressLine2, string addressLine3, long pinCode, string phoneNumber)
    {
        if (string.IsNullOrWhiteSpace(addressLine1)) throw new ArgumentException("Address line 1 is required.", nameof(addressLine1));
        if (pinCode <= 0) throw new ArgumentException("Pin code must be positive.", nameof(pinCode));

        LocationCode = locationCode;
        AddressLine1 = addressLine1;
        AddressLine2 = addressLine2 ?? string.Empty;
        AddressLine3 = addressLine3 ?? string.Empty;
        PinCode = pinCode;
        PhoneNumber = phoneNumber ?? string.Empty;
    }
}
