using BusServices.Domain.Common;
using BusServices.Domain.Events;
using BusServices.Domain.Exceptions;
using BusServices.Domain.ValueObjects;

namespace BusServices.Domain.Entities;

/// <summary>
/// Aggregate root for the Bus aggregate. Maps to BUS_MASTER table.
/// </summary>
public sealed class Bus : BaseEntity
{
    private readonly List<BusRoute> _routes = new();
    private readonly List<BusArrival> _arrivals = new();
    private readonly List<BusDeductionRate> _deductionRates = new();

    public int BusId { get; private set; }
    public RegistrationNumber RegistrationNumber { get; private set; } = null!;
    public string? Description { get; private set; }
    public int Capacity { get; private set; }
    public int? CapacityReserved { get; private set; }
    public DateTime OperatingFrom { get; private set; }
    public long LastModifiedBy { get; private set; }
    public DateTime LastModifiedOn { get; private set; }

    public IReadOnlyCollection<BusRoute> Routes => _routes.AsReadOnly();
    public IReadOnlyCollection<BusArrival> Arrivals => _arrivals.AsReadOnly();
    public IReadOnlyCollection<BusDeductionRate> DeductionRates => _deductionRates.AsReadOnly();

    private Bus() { }

    public static Bus Register(
        int busId,
        string registrationNumber,
        string? description,
        int capacity,
        long registeredBy)
    {
        if (capacity <= 0)
            throw new DomainException("Bus capacity must be greater than zero.");

        var bus = new Bus
        {
            BusId = busId,
            RegistrationNumber = RegistrationNumber.Create(registrationNumber),
            Description = description,
            Capacity = capacity,
            OperatingFrom = DateTime.UtcNow,
            LastModifiedBy = registeredBy,
            LastModifiedOn = DateTime.UtcNow
        };

        bus.AddDomainEvent(new BusRegisteredEvent(bus.BusId, bus.RegistrationNumber.Value, registeredBy));
        return bus;
    }

    public void UpdateDetails(string? description, int capacity, long modifiedBy)
    {
        if (capacity <= 0)
            throw new DomainException("Bus capacity must be greater than zero.");

        Description = description;
        Capacity = capacity;
        LastModifiedBy = modifiedBy;
        LastModifiedOn = DateTime.UtcNow;
    }

    public void SetReservedCapacity(int reserved, long modifiedBy)
    {
        if (reserved < 0 || reserved > Capacity)
            throw new DomainException($"Reserved capacity must be between 0 and {Capacity}.");

        CapacityReserved = reserved;
        LastModifiedBy = modifiedBy;
        LastModifiedOn = DateTime.UtcNow;
    }

    public BusRoute AddRoute(int routeId, string name, string? description, long createdBy)
    {
        var route = BusRoute.Create(routeId, BusId, name, description, createdBy);
        _routes.Add(route);
        return route;
    }

    public BusArrival RecordArrival(long arrivalId, DateTime date, TimeOnly time, char status, string? remarks, long recordedBy)
    {
        var arrival = BusArrival.Record(arrivalId, BusId, date, time, status, remarks, recordedBy);
        _arrivals.Add(arrival);
        AddDomainEvent(new BusArrivedEvent(arrivalId, BusId, date, time, status));
        return arrival;
    }
}
