using AdminService.Domain.Events;

namespace AdminService.Domain.Aggregates;

/// <summary>
/// Finance unit aggregate root
/// </summary>
public class FinanceUnitAggregate
{
    private List<DomainEvent> _domainEvents = new();

    public long Id { get; private set; }
    public long UnitId { get; private set; }
    public string? UnitCode { get; private set; }
    public string Name { get; private set; } = string.Empty;
    public long? OracleCode { get; private set; }
    public string? LocationOption { get; private set; }

    /// <summary>
    /// Get all domain events
    /// </summary>
    public IReadOnlyCollection<DomainEvent> GetDomainEvents() => _domainEvents.AsReadOnly();

    /// <summary>
    /// Clear all domain events
    /// </summary>
    public void ClearDomainEvents() => _domainEvents.Clear();

    /// <summary>
    /// Create a new finance unit
    /// </summary>
    public static FinanceUnitAggregate Create(long unitId, string? unitCode, string name, long? oracleCode)
    {
        var aggregate = new FinanceUnitAggregate
        {
            UnitId = unitId,
            UnitCode = unitCode,
            Name = name ?? throw new ArgumentNullException(nameof(name)),
            OracleCode = oracleCode,
            LocationOption = "N"
        };

        aggregate._domainEvents.Add(new FinanceUnitCreatedEvent(
            unitId,
            unitCode ?? string.Empty,
            name,
            DateTime.UtcNow
        ));

        return aggregate;
    }

    /// <summary>
    /// Update finance unit
    /// </summary>
    public void Update(string name, long? oracleCode, string? locationOption)
    {
        Name = name ?? throw new ArgumentNullException(nameof(name));
        OracleCode = oracleCode;
        LocationOption = locationOption;
    }
}
