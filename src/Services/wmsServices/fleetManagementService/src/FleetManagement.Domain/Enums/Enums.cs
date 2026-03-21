namespace FleetManagement.Domain.Enums;

public enum VehicleType
{
    TRUCK,
    FORKLIFT,
    PALLET_JACK,
    VAN,
    OTHER
}

public enum VehicleStatus
{
    AVAILABLE,
    IN_USE,
    MAINTENANCE,
    RETIRED
}

public enum TripStatus
{
    PLANNED,
    IN_PROGRESS,
    COMPLETED,
    CANCELLED
}

public enum LocationType
{
    WAREHOUSE,
    CUSTOMER,
    SUPPLIER
}

public enum StopStatus
{
    PENDING,
    ARRIVED,
    DEPARTED,
    SKIPPED
}
