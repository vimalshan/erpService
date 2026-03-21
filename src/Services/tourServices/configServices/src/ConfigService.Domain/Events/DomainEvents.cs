using ConfigService.Domain.Common;

namespace ConfigService.Domain.Events;

public record CurrencyCreatedEvent(long CurrencyId, string CurrencyCode) : IDomainEvent
{
    public DateTime OccurredOn { get; } = DateTime.UtcNow;
}

public record CountryCreatedEvent(string CountryId, string CountryName) : IDomainEvent
{
    public DateTime OccurredOn { get; } = DateTime.UtcNow;
}

public record CityCreatedEvent(string CityId, string CityName) : IDomainEvent
{
    public DateTime OccurredOn { get; } = DateTime.UtcNow;
}

public record VendorCreatedEvent(string VendorId, string VendorName) : IDomainEvent
{
    public DateTime OccurredOn { get; } = DateTime.UtcNow;
}

public record VendorUpdatedEvent(string VendorId, string VendorName) : IDomainEvent
{
    public DateTime OccurredOn { get; } = DateTime.UtcNow;
}

public record ExpenseRuleChangedEvent(string RuleId, string GradeCategory) : IDomainEvent
{
    public DateTime OccurredOn { get; } = DateTime.UtcNow;
}

public record ConfigurationChangedEvent(string EntityType, string EntityId, string Action) : IDomainEvent
{
    public DateTime OccurredOn { get; } = DateTime.UtcNow;
}
