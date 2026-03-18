using CalendarService.Infrastructure.Messaging;
using MassTransit;
using Microsoft.Extensions.Logging;

namespace CalendarService.Infrastructure.Messaging.Consumers;

public class CalendarCreatedConsumer(ILogger<CalendarCreatedConsumer> logger)
    : IConsumer<CalendarCreatedMessage>
{
    public Task Consume(ConsumeContext<CalendarCreatedMessage> context)
    {
        logger.LogInformation("[Consumer] Calendar created: {Id} - {Name}", context.Message.CalendarId, context.Message.CalendarName);
        return Task.CompletedTask;
    }
}

public class HolidayCreatedConsumer(ILogger<HolidayCreatedConsumer> logger)
    : IConsumer<HolidayCreatedMessage>
{
    public Task Consume(ConsumeContext<HolidayCreatedMessage> context)
    {
        logger.LogInformation("[Consumer] Holiday created: {Id} on {Date}", context.Message.HolidayId, context.Message.HolidayDate);
        return Task.CompletedTask;
    }
}

public class ShiftCreatedConsumer(ILogger<ShiftCreatedConsumer> logger)
    : IConsumer<ShiftCreatedMessage>
{
    public Task Consume(ConsumeContext<ShiftCreatedMessage> context)
    {
        logger.LogInformation("[Consumer] Shift created: {Code} - {Name}", context.Message.ShiftCode, context.Message.ShiftName);
        return Task.CompletedTask;
    }
}
