using TdsService.Domain.Common;
using TdsService.Domain.Events;
using TdsService.Domain.ValueObjects;

namespace TdsService.Domain.Entities;

/// <summary>
/// Aggregate root representing a TDS vendor.
/// Maps to the TDS_VENDORS table (vendor PAN links to TDSFILE_DETAILS).
/// </summary>
public sealed class TdsVendor : AggregateRoot<long>
{
    public string VendorName { get; private set; } = string.Empty;
    public EmailAddress? EmailAddress { get; private set; }
    public PanNumber? PanNumber { get; private set; }

    // Navigation — a vendor can have many TDS files
    private readonly List<TdsFile> _files = [];
    public IReadOnlyList<TdsFile> Files => _files.AsReadOnly();

    private TdsVendor() { }

    public static TdsVendor Create(
        long vendorId,
        string vendorName,
        string? emailAddress,
        string? panNo)
    {
        var vendor = new TdsVendor
        {
            Id = vendorId,
            VendorName = vendorName,
            EmailAddress = emailAddress is not null ? EmailAddress.TryCreate(emailAddress) : null,
            PanNumber = panNo is not null ? PanNumber.TryCreate(panNo) : null
        };

        vendor.RaiseDomainEvent(new TdsVendorCreatedEvent(vendorId, vendorName, panNo));
        return vendor;
    }

    public void Update(string vendorName, string? emailAddress, string? panNo)
    {
        VendorName = vendorName;
        EmailAddress = emailAddress is not null ? EmailAddress.TryCreate(emailAddress) : null;
        PanNumber = panNo is not null ? PanNumber.TryCreate(panNo) : null;

        RaiseDomainEvent(new TdsVendorUpdatedEvent(Id, vendorName, panNo));
    }
}
