using VendorService.Domain.Common;

namespace VendorService.Domain.Entities;

public sealed class TdsVendor : Entity
{
    public long? VendorId { get; private set; }
    public string? VendorName { get; private set; }
    public string? EmailAddress { get; private set; }
    public string? PanNo { get; private set; }

    private TdsVendor() { }

    public static TdsVendor Create(long? vendorId, string? vendorName, string? emailAddress, string? panNo)
    {
        return new TdsVendor
        {
            VendorId = vendorId,
            VendorName = vendorName?.Trim(),
            EmailAddress = emailAddress?.Trim(),
            PanNo = panNo?.Trim()
        };
    }

    public void UpdateEmailAddress(string emailAddress)
    {
        EmailAddress = emailAddress?.Trim();
    }
}
