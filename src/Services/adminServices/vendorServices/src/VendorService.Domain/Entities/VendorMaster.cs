using VendorService.Domain.Common;
using VendorService.Domain.Events;
using VendorService.Domain.Exceptions;
using VendorService.Domain.ValueObjects;

namespace VendorService.Domain.Entities;

public sealed class VendorMaster : AggregateRoot
{
    public long Id { get; private set; }
    public long CategoryId { get; private set; }
    public long LocationId { get; private set; }
    public VendorName Name { get; private set; } = null!;
    public Email? Email { get; private set; }
    public Address Address { get; private set; } = null!;
    public long UpdatedBy { get; private set; }
    public DateTime UpdatedOn { get; private set; }
    public LiveStatus LiveStatus { get; private set; } = null!;

    private VendorMaster() { }

    public static VendorMaster Create(
        long id,
        long categoryId,
        long locationId,
        string name,
        string? email,
        string address,
        long updatedBy,
        char liveStatus = 'A')
    {
        if (categoryId <= 0) throw new VendorDomainException("Category ID must be positive.");
        if (locationId <= 0) throw new VendorDomainException("Location ID must be positive.");
        if (updatedBy <= 0) throw new VendorDomainException("UpdatedBy must be a valid user ID.");

        var vendor = new VendorMaster
        {
            Id = id,
            CategoryId = categoryId,
            LocationId = locationId,
            Name = new VendorName(name),
            Email = string.IsNullOrWhiteSpace(email) ? null : new Email(email),
            Address = new Address(address),
            UpdatedBy = updatedBy,
            UpdatedOn = DateTime.UtcNow,
            LiveStatus = new LiveStatus(liveStatus)
        };

        vendor.RaiseDomainEvent(new VendorCreatedEvent(id, name, address, locationId, categoryId));
        return vendor;
    }

    public void Update(
        long categoryId,
        long locationId,
        string name,
        string? email,
        string address,
        long updatedBy,
        char liveStatus)
    {
        if (categoryId <= 0) throw new VendorDomainException("Category ID must be positive.");
        if (locationId <= 0) throw new VendorDomainException("Location ID must be positive.");

        bool statusChanged = LiveStatus.Value != char.ToUpperInvariant(liveStatus);

        CategoryId = categoryId;
        LocationId = locationId;
        Name = new VendorName(name);
        Email = string.IsNullOrWhiteSpace(email) ? null : new Email(email);
        Address = new Address(address);
        UpdatedBy = updatedBy;
        UpdatedOn = DateTime.UtcNow;
        LiveStatus = new LiveStatus(liveStatus);

        RaiseDomainEvent(new VendorUpdatedEvent(Id, name));

        if (statusChanged)
            RaiseDomainEvent(new VendorStatusChangedEvent(Id, liveStatus));
    }

    public void Deactivate(long updatedBy)
    {
        if (!LiveStatus.IsActive)
            throw new VendorDomainException("Vendor is already inactive.");

        LiveStatus = LiveStatus.Inactive;
        UpdatedBy = updatedBy;
        UpdatedOn = DateTime.UtcNow;
        RaiseDomainEvent(new VendorStatusChangedEvent(Id, 'I'));
    }
}
