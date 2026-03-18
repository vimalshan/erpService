using TrustService.Domain.Common;
using TrustService.Domain.Events;
using TrustService.Domain.ValueObjects;

namespace TrustService.Domain.Entities;

public class TrustMaster : AggregateRoot
{
    public string TrustCode { get; private set; } = string.Empty;
    public string TrustShortName { get; private set; } = string.Empty;
    public string TrustType { get; private set; } = string.Empty;
    public DateTime TrustStartDate { get; private set; }
    public DateTime? TrustClosureDate { get; private set; }
    public string? TrustId { get; private set; }
    public string AddressLine1 { get; private set; } = string.Empty;
    public string? AddressLine2 { get; private set; }
    public string? AddressLine3 { get; private set; }
    public string? City { get; private set; }
    public string? State { get; private set; }
    public string? PinCode { get; private set; }
    public string? Country { get; private set; }
    public string? PhoneNo { get; private set; }
    public string? FaxNo { get; private set; }
    public string? Email { get; private set; }
    public string TrustStatus { get; private set; } = "A";
    public DateTime CreatedDate { get; private set; }
    public DateTime? UpdatedDate { get; private set; }
    public string? RegistrarName { get; private set; }
    public string? RegistrarPhone { get; private set; }

    // Navigation properties
    public ICollection<TrustFundType> FundTypes { get; private set; } = new List<TrustFundType>();
    public ICollection<TrustRole> Roles { get; private set; } = new List<TrustRole>();
    public ICollection<TrustApprover> Approvers { get; private set; } = new List<TrustApprover>();
    public ICollection<TrustConfiguration> Configurations { get; private set; } = new List<TrustConfiguration>();
    public ICollection<TrustAuditLog> AuditLogs { get; private set; } = new List<TrustAuditLog>();
    public ICollection<TrustUnit> Units { get; private set; } = new List<TrustUnit>();

    private TrustMaster() { }

    public static TrustMaster Create(string trustCode, string trustShortName, string trustType,
        DateTime startDate, Address address, ContactInfo? contact = null,
        string? registrarName = null, string? registrarPhone = null)
    {
        if (string.IsNullOrWhiteSpace(trustCode) || trustCode.Length > 3)
            throw new ArgumentException("Trust code must be 1-3 characters.", nameof(trustCode));

        var trust = new TrustMaster
        {
            TrustCode = trustCode.ToUpperInvariant(),
            TrustShortName = trustShortName,
            TrustType = trustType,
            TrustStartDate = startDate,
            AddressLine1 = address.AddressLine1,
            AddressLine2 = address.AddressLine2,
            AddressLine3 = address.AddressLine3,
            City = address.City,
            State = address.State,
            PinCode = address.PinCode,
            Country = address.Country,
            PhoneNo = contact?.PhoneNo,
            FaxNo = contact?.FaxNo,
            Email = contact?.Email,
            TrustStatus = "A",
            CreatedDate = DateTime.UtcNow,
            RegistrarName = registrarName,
            RegistrarPhone = registrarPhone
        };

        trust.AddDomainEvent(new TrustCreatedEvent(trust.TrustCode, trust.TrustShortName));
        return trust;
    }

    public void Update(string trustShortName, Address address, ContactInfo? contact = null,
        string? registrarName = null, string? registrarPhone = null)
    {
        TrustShortName = trustShortName;
        AddressLine1 = address.AddressLine1;
        AddressLine2 = address.AddressLine2;
        AddressLine3 = address.AddressLine3;
        City = address.City;
        State = address.State;
        PinCode = address.PinCode;
        Country = address.Country;
        PhoneNo = contact?.PhoneNo;
        FaxNo = contact?.FaxNo;
        Email = contact?.Email;
        RegistrarName = registrarName;
        RegistrarPhone = registrarPhone;
        UpdatedDate = DateTime.UtcNow;

        AddDomainEvent(new TrustUpdatedEvent(TrustCode, TrustShortName));
    }

    public void Close(DateTime closureDate)
    {
        TrustClosureDate = closureDate;
        TrustStatus = "C";
        UpdatedDate = DateTime.UtcNow;
        AddDomainEvent(new TrustClosedEvent(TrustCode));
    }

    public void Activate()
    {
        TrustStatus = "A";
        TrustClosureDate = null;
        UpdatedDate = DateTime.UtcNow;
        AddDomainEvent(new TrustStatusChangedEvent(TrustCode, "A"));
    }

    public TrustFundType AddFundType(string fundType, string fundName, string fundPrefix)
    {
        var fund = TrustFundType.Create(TrustCode, fundType, fundName, fundPrefix);
        FundTypes.Add(fund);
        AddDomainEvent(new TrustFundTypeAddedEvent(TrustCode, fundType, fundName));
        return fund;
    }

    public TrustRole AddRole(int roleId, string roleCode, string userId, long userNo)
    {
        var role = TrustRole.Create(TrustCode, roleId, roleCode, userId, userNo);
        Roles.Add(role);
        AddDomainEvent(new TrustRoleAssignedEvent(TrustCode, userId, roleCode));
        return role;
    }

    public TrustUnit AddUnit(string unitCode, string unitName, string unitType,
        string addressLine1, string? addressLine2, string city, string state, long? unitHeadSysId = null)
    {
        var unit = TrustUnit.Create(TrustCode, unitCode, unitName, unitType,
            addressLine1, addressLine2, city, state, unitHeadSysId);
        Units.Add(unit);
        AddDomainEvent(new TrustUnitAddedEvent(TrustCode, unitCode, unitName));
        return unit;
    }
}
