using CustomerService.Domain.Common;
using CustomerService.Domain.Events;
using CustomerService.Domain.ValueObjects;

namespace CustomerService.Domain.Entities;

public class Customer : BaseEntity
{
    public int CustomerId { get; private set; }
    public string Code { get; private set; } = string.Empty;
    public string Name { get; private set; } = string.Empty;
    public string? CompanyName { get; private set; }
    public ContactInfo Contact { get; private set; } = new();
    public Address Address { get; private set; } = new();
    public bool IsActive { get; private set; } = true;

    private Customer() { } // EF constructor

    public static Customer Create(string code, string name, string? companyName,
        ContactInfo contact, Address address)
    {
        var customer = new Customer
        {
            Code = code,
            Name = name,
            CompanyName = companyName,
            Contact = contact,
            Address = address,
            IsActive = true,
            CreatedDate = DateTime.UtcNow,
            ModifiedDate = DateTime.UtcNow
        };

        customer.AddDomainEvent(new CustomerCreatedEvent(customer));
        return customer;
    }

    public void Update(string name, string? companyName, ContactInfo contact, Address address)
    {
        Name = name;
        CompanyName = companyName;
        Contact = contact;
        Address = address;
        ModifiedDate = DateTime.UtcNow;

        AddDomainEvent(new CustomerUpdatedEvent(this));
    }

    public void Activate()
    {
        IsActive = true;
        ModifiedDate = DateTime.UtcNow;
        AddDomainEvent(new CustomerActivatedEvent(CustomerId));
    }

    public void Deactivate()
    {
        IsActive = false;
        ModifiedDate = DateTime.UtcNow;
        AddDomainEvent(new CustomerDeactivatedEvent(CustomerId));
    }
}
