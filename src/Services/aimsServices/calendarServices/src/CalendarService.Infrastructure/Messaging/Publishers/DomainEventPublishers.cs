using CalendarService.Domain.Events;
using CalendarService.Infrastructure.Messaging;
using MassTransit;
using MediatR;

namespace CalendarService.Infrastructure.Messaging.Publishers;

public class CalendarCreatedEventHandler(IPublishEndpoint bus) : INotificationHandler<CalendarCreatedEvent>
{
    public async Task Handle(CalendarCreatedEvent notification, CancellationToken ct)
        => await bus.Publish(new CalendarCreatedMessage(notification.CalendarId, notification.CalendarName, notification.OccurredOn), ct);
}

public class HolidayCreatedEventHandler(IPublishEndpoint bus) : INotificationHandler<HolidayCreatedEvent>
{
    public async Task Handle(HolidayCreatedEvent notification, CancellationToken ct)
        => await bus.Publish(new HolidayCreatedMessage(notification.HolidayId, notification.HolidayDate, notification.Description, notification.OccurredOn), ct);
}

public class ShiftCreatedEventHandler(IPublishEndpoint bus) : INotificationHandler<ShiftCreatedEvent>
{
    public async Task Handle(ShiftCreatedEvent notification, CancellationToken ct)
        => await bus.Publish(new ShiftCreatedMessage(notification.ShiftId, notification.ShiftCode, notification.ShiftName, notification.OccurredOn), ct);
}
