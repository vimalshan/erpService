using IntegrationService.Domain.Common;
using IntegrationService.Domain.Events;

namespace IntegrationService.Domain.Entities;

public class Vendor : BaseEntity<int>, IAggregateRoot
{
    public string VendorName { get; private set; } = string.Empty;
    public string VendorCode { get; private set; } = string.Empty;

    private readonly List<VendorSite> _vendorSites = [];
    public IReadOnlyCollection<VendorSite> VendorSites => _vendorSites.AsReadOnly();

    private Vendor() { }

    public static Vendor Create(int vendorId, string vendorName, string vendorCode)
    {
        if (string.IsNullOrWhiteSpace(vendorName))
            throw new ArgumentException("Vendor name is required.", nameof(vendorName));
        if (string.IsNullOrWhiteSpace(vendorCode))
            throw new ArgumentException("Vendor code is required.", nameof(vendorCode));

        var vendor = new Vendor
        {
            Id = vendorId,
            VendorName = vendorName,
            VendorCode = vendorCode
        };

        vendor.AddDomainEvent(new VendorCreatedEvent(vendor.Id, vendor.VendorName));
        return vendor;
    }

    public void UpdateDetails(string vendorName, string vendorCode)
    {
        VendorName = vendorName;
        VendorCode = vendorCode;
        AddDomainEvent(new VendorUpdatedEvent(Id, VendorName));
    }

    public void AddVendorSite(VendorSite site)
    {
        _vendorSites.Add(site);
    }
}
