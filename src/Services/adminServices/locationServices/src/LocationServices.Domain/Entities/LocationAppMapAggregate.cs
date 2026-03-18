using LocationServices.Domain.Common;
using LocationServices.Domain.Events;
using LocationServices.Domain.ValueObjects;
using System.ComponentModel.DataAnnotations.Schema;

namespace LocationServices.Domain.Entities;

/// <summary>
/// Aggregate Root — LocationAppMap
/// DDD: Encapsulates business rules for location-app access mapping
/// </summary>
[Table("LOCATION_APP_MAP")]
public sealed class LocationAppMapAggregate : Entity<(decimal LocationId, string AppName)>
{
    // Private constructor for EF Core
    private LocationAppMapAggregate() { }

    public decimal LocationId { get; private set; }
    public string AppName     { get; private set; } = null!;
    public long?  SiteCategoryCode { get; private set; }
    public string? SelfAccess      { get; private set; }
    public string? DeemedApproval  { get; private set; }
    public string? CreatedBy       { get; private set; }
    public string? ModifiedBy      { get; private set; }
    public DateTime? ModifiedDate  { get; private set; }
    public bool IsActive           { get; private set; } = true;

    /// <summary>Factory method — the only valid way to create a new mapping</summary>
    public static LocationAppMapAggregate Create(
        LocationId locationId,
        AppName appName,
        SiteCategoryCode? siteCategoryCode,
        string? selfAccess,
        string? deemedApproval,
        string createdBy)
    {
        var mapping = new LocationAppMapAggregate
        {
            LocationId       = locationId.Value,
            AppName          = appName.Value,
            SiteCategoryCode = siteCategoryCode?.Value,
            SelfAccess       = selfAccess,
            DeemedApproval   = deemedApproval,
            CreatedBy        = createdBy,
            IsActive         = true,
            Id               = (locationId.Value, appName.Value)
        };

        // Raise domain event
        mapping.RaiseDomainEvent(new LocationAppMapCreatedEvent(
            mapping.LocationId, mapping.AppName, createdBy));

        return mapping;
    }

    /// <summary>Update mapping — validates and raises event</summary>
    public void Update(
        SiteCategoryCode? siteCategoryCode,
        string? selfAccess,
        string? deemedApproval,
        bool isActive,
        string modifiedBy)
    {
        var old = (SiteCategoryCode, SelfAccess, DeemedApproval, IsActive);

        SiteCategoryCode = siteCategoryCode?.Value;
        SelfAccess       = selfAccess;
        DeemedApproval   = deemedApproval;
        IsActive         = isActive;
        ModifiedBy       = modifiedBy;
        ModifiedDate     = DateTime.UtcNow;

        RaiseDomainEvent(new LocationAppMapUpdatedEvent(
            LocationId, AppName, modifiedBy));
    }

    /// <summary>Soft-delete</summary>
    public void Deactivate(string modifiedBy)
    {
        IsActive     = false;
        ModifiedBy   = modifiedBy;
        ModifiedDate = DateTime.UtcNow;

        RaiseDomainEvent(new LocationAppMapDeletedEvent(LocationId, AppName, modifiedBy));
    }
}
