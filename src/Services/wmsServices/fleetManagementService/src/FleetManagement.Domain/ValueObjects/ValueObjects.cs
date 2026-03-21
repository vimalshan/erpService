namespace FleetManagement.Domain.ValueObjects;

public record Capacity(decimal Weight, decimal Volume);

public record Address(string FullAddress);

public record LicenseInfo(string LicenseNumber, DateTime LicenseExpiry);

public record DateRange(DateTime Start, DateTime End);

public record OdometerReading(int Value);
