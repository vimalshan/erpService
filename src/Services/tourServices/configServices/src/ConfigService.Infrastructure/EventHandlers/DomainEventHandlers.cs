using ConfigService.Domain.Common;
using ConfigService.Domain.Events;
using MediatR;
using Microsoft.Extensions.Logging;

namespace ConfigService.Infrastructure.EventHandlers;

public class VendorCreatedEventHandler(ILogger<VendorCreatedEventHandler> logger) : INotificationHandler<VendorCreatedEvent>
{
    public Task Handle(VendorCreatedEvent notification, CancellationToken ct)
    {
        logger.LogInformation("Domain Event: Vendor created - {VendorId} ({VendorName})",
            notification.VendorId, notification.VendorName);
        return Task.CompletedTask;
    }
}

public class VendorUpdatedEventHandler(ILogger<VendorUpdatedEventHandler> logger) : INotificationHandler<VendorUpdatedEvent>
{
    public Task Handle(VendorUpdatedEvent notification, CancellationToken ct)
    {
        logger.LogInformation("Domain Event: Vendor updated - {VendorId} ({VendorName})",
            notification.VendorId, notification.VendorName);
        return Task.CompletedTask;
    }
}

public class CurrencyCreatedEventHandler(ILogger<CurrencyCreatedEventHandler> logger) : INotificationHandler<CurrencyCreatedEvent>
{
    public Task Handle(CurrencyCreatedEvent notification, CancellationToken ct)
    {
        logger.LogInformation("Domain Event: Currency created - {CurrencyId} ({CurrencyCode})",
            notification.CurrencyId, notification.CurrencyCode);
        return Task.CompletedTask;
    }
}

public class CountryCreatedEventHandler(ILogger<CountryCreatedEventHandler> logger) : INotificationHandler<CountryCreatedEvent>
{
    public Task Handle(CountryCreatedEvent notification, CancellationToken ct)
    {
        logger.LogInformation("Domain Event: Country created - {CountryId} ({CountryName})",
            notification.CountryId, notification.CountryName);
        return Task.CompletedTask;
    }
}

public class CityCreatedEventHandler(ILogger<CityCreatedEventHandler> logger) : INotificationHandler<CityCreatedEvent>
{
    public Task Handle(CityCreatedEvent notification, CancellationToken ct)
    {
        logger.LogInformation("Domain Event: City created - {CityId} ({CityName})",
            notification.CityId, notification.CityName);
        return Task.CompletedTask;
    }
}

public class ConfigurationChangedEventHandler(ILogger<ConfigurationChangedEventHandler> logger) : INotificationHandler<ConfigurationChangedEvent>
{
    public Task Handle(ConfigurationChangedEvent notification, CancellationToken ct)
    {
        logger.LogInformation("Domain Event: Configuration changed - {EntityType}/{EntityId} ({Action})",
            notification.EntityType, notification.EntityId, notification.Action);
        return Task.CompletedTask;
    }
}
