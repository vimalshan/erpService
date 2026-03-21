using AdminService.Domain.Events;

namespace AdminService.Domain.Aggregates;

/// <summary>
/// Admin unit aggregate root
/// </summary>
public class AdminUnitAggregate
{
    private List<DomainEvent> _domainEvents = new();

    public long Id { get; private set; }
    public long AdminCode { get; private set; }
    public string Name { get; private set; } = string.Empty;
    public string? AdminType { get; private set; }
    public string? UnitCode { get; private set; }
    public long? CabUnit { get; private set; }
    public string? ImageUrl { get; private set; }
    public long? SortOrder { get; private set; }

    /// <summary>
    /// Get all domain events
    /// </summary>
    public IReadOnlyCollection<DomainEvent> GetDomainEvents() => _domainEvents.AsReadOnly();

    /// <summary>
    /// Clear all domain events
    /// </summary>
    public void ClearDomainEvents() => _domainEvents.Clear();

    /// <summary>
    /// Create a new admin unit
    /// </summary>
    public static AdminUnitAggregate Create(long adminCode, string name, string? adminType, string? unitCode)
    {
        var aggregate = new AdminUnitAggregate
        {
            AdminCode = adminCode,
            Name = name ?? throw new ArgumentNullException(nameof(name)),
            AdminType = adminType,
            UnitCode = unitCode
        };

        aggregate._domainEvents.Add(new AdminUnitCreatedEvent(
            adminCode,
            name,
            adminType,
            DateTime.UtcNow
        ));

        return aggregate;
    }

    /// <summary>
    /// Update admin unit details
    /// </summary>
    public void Update(string name, string? adminType, string? unitCode, long? cabUnit, string? imageUrl, long? sortOrder)
    {
        Name = name ?? throw new ArgumentNullException(nameof(name));
        AdminType = adminType;
        UnitCode = unitCode;
        CabUnit = cabUnit;
        ImageUrl = imageUrl;
        SortOrder = sortOrder;

        _domainEvents.Add(new AdminUnitUpdatedEvent(
            AdminCode,
            Name,
            AdminType,
            DateTime.UtcNow
        ));
    }

    /// <summary>
    /// Delete admin unit
    /// </summary>
    public void Delete()
    {
        _domainEvents.Add(new AdminUnitDeletedEvent(
            AdminCode,
            DateTime.UtcNow
        ));
    }
}
