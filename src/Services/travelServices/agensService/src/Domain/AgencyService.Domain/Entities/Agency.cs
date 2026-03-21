using AgencyService.Domain.Common;
using AgencyService.Domain.ValueObjects;

namespace AgencyService.Domain.Entities;

public class Agency : AggregateRoot
{
    public long AgencyCode { get; private set; }
    public string Name { get; private set; }
    public AgencyType Type { get; private set; }
    public ContactInfo ContactInfo { get; private set; }
    public Address Address { get; private set; }
    public long? AdminUnit { get; private set; }
    public string? OracleCode { get; private set; }
    public string? OracleSite { get; private set; }
    public long? TerminalId { get; private set; }
    public bool? LateCABFlag { get; private set; }
    public string? R12BUCode { get; private set; }
    public string? R12Location { get; private set; }
    public string? OracleItemCode { get; private set; }
    public bool? GSTRecover { get; private set; }
    public long? ModifiedBy { get; private set; }
    public DateTime? ModifiedOn { get; private set; }
    public DateTime CreatedOn { get; private set; }
    
    public Agency(
        long agencyCode,
        string name,
        AgencyType type,
        ContactInfo contactInfo,
        Address address)
    {
        if (agencyCode <= 0)
            throw new ArgumentException("Agency code must be greater than 0", nameof(agencyCode));
            
        if (string.IsNullOrWhiteSpace(name))
            throw new ArgumentException("Agency name cannot be empty", nameof(name));
            
        AgencyCode = agencyCode;
        Name = name;
        Type = type ?? throw new ArgumentNullException(nameof(type));
        ContactInfo = contactInfo ?? throw new ArgumentNullException(nameof(contactInfo));
        Address = address ?? throw new ArgumentNullException(nameof(address));
        CreatedOn = DateTime.UtcNow;
        
        AddDomainEvent(new AgencyCreatedEvent(agencyCode, name));
    }
    
    public void Update(
        string name,
        ContactInfo contactInfo,
        long modifiedBy)
    {
        if (string.IsNullOrWhiteSpace(name))
            throw new ArgumentException("Agency name cannot be empty", nameof(name));
            
        Name = name;
        ContactInfo = contactInfo ?? throw new ArgumentNullException(nameof(contactInfo));
        ModifiedBy = modifiedBy;
        ModifiedOn = DateTime.UtcNow;
        
        AddDomainEvent(new AgencyUpdatedEvent(AgencyCode, name));
    }
    
    private Agency() { }
}

public class AgencyCreatedEvent : DomainEvent
{
    public long AgencyCode { get; set; }
    public string AgencyName { get; set; }
    
    public AgencyCreatedEvent(long agencyCode, string agencyName)
    {
        AgencyCode = agencyCode;
        AgencyName = agencyName;
    }
}

public class AgencyUpdatedEvent : DomainEvent
{
    public long AgencyCode { get; set; }
    public string AgencyName { get; set; }
    
    public AgencyUpdatedEvent(long agencyCode, string agencyName)
    {
        AgencyCode = agencyCode;
        AgencyName = agencyName;
    }
}
