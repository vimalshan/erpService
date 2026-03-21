using SupplierService.Domain.Common;
using SupplierService.Domain.Events;
using SupplierService.Domain.ValueObjects;

namespace SupplierService.Domain.Entities;

public class Supplier : BaseEntity, IAggregateRoot
{
    public int SupplierId { get; private set; }
    public string Code { get; private set; } = string.Empty;
    public string Name { get; private set; } = string.Empty;
    public string? ContactPerson { get; private set; }
    public string? Email { get; private set; }
    public string? Phone { get; private set; }
    public Address Address { get; private set; } = null!;
    public bool IsActive { get; private set; } = true;
    public DateTime CreatedDate { get; private set; }
    public DateTime ModifiedDate { get; private set; }

    private Supplier() { }

    public static Supplier Create(string code, string name, string? contactPerson, string? email,
        string? phone, Address address)
    {
        var supplier = new Supplier
        {
            Code = code,
            Name = name,
            ContactPerson = contactPerson,
            Email = email,
            Phone = phone,
            Address = address,
            IsActive = true,
            CreatedDate = DateTime.UtcNow,
            ModifiedDate = DateTime.UtcNow
        };

        supplier.AddDomainEvent(new SupplierCreatedEvent(supplier));
        return supplier;
    }

    public void Update(string name, string? contactPerson, string? email, string? phone, Address address)
    {
        Name = name;
        ContactPerson = contactPerson;
        Email = email;
        Phone = phone;
        Address = address;
        ModifiedDate = DateTime.UtcNow;

        AddDomainEvent(new SupplierUpdatedEvent(this));
    }

    public void Activate()
    {
        if (IsActive) return;
        IsActive = true;
        ModifiedDate = DateTime.UtcNow;
        AddDomainEvent(new SupplierActivatedEvent(SupplierId));
    }

    public void Deactivate()
    {
        if (!IsActive) return;
        IsActive = false;
        ModifiedDate = DateTime.UtcNow;
        AddDomainEvent(new SupplierDeactivatedEvent(SupplierId));
    }
}
