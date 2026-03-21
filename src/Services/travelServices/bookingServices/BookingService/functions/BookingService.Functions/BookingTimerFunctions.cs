using Microsoft.Azure.Functions.Worker;
using Microsoft.Extensions.Logging;

namespace BookingService.Functions;

/// <summary>
/// Timer-triggered function: runs every hour to clean up expired/stale bookings.
/// </summary>
public class BookingCleanupFunction
{
    private readonly ILogger<BookingCleanupFunction> _logger;

    public BookingCleanupFunction(ILogger<BookingCleanupFunction> logger)
        => _logger = logger;

    [Function("BookingCleanup")]
    public async Task Run(
        [TimerTrigger("0 0 * * * *")] TimerInfo timerInfo, // every hour
        FunctionContext context)
    {
        _logger.LogInformation("BookingCleanup triggered at: {Time}", DateTime.UtcNow);

        // TODO: wire up IBookingRepository via DI and query for stale/expired bookings
        // Example: bookings in 'N' status older than 30 days with no confirmation
        await Task.Delay(100); // placeholder

        _logger.LogInformation("BookingCleanup completed. Next run: {Next}", timerInfo.ScheduleStatus?.Next);
    }
}

/// <summary>
/// Timer-triggered function: generates daily booking summary reports.
/// </summary>
public class DailyBookingReportFunction
{
    private readonly ILogger<DailyBookingReportFunction> _logger;

    public DailyBookingReportFunction(ILogger<DailyBookingReportFunction> logger)
        => _logger = logger;

    [Function("DailyBookingReport")]
    public async Task Run(
        [TimerTrigger("0 0 6 * * *")] TimerInfo timerInfo, // daily at 6AM UTC
        FunctionContext context)
    {
        _logger.LogInformation("DailyBookingReport triggered at: {Time}", DateTime.UtcNow);

        // TODO: query booking stats, generate report, send via email or store in Blob Storage
        await Task.Delay(100); // placeholder

        _logger.LogInformation("DailyBookingReport completed");
    }
}

/// <summary>
/// Timer-triggered function: sends reminders for upcoming travel.
/// </summary>
public class TravelReminderFunction
{
    private readonly ILogger<TravelReminderFunction> _logger;

    public TravelReminderFunction(ILogger<TravelReminderFunction> logger)
        => _logger = logger;

    [Function("TravelReminder")]
    public async Task Run(
        [TimerTrigger("0 0 8 * * *")] TimerInfo timerInfo, // daily at 8AM UTC
        FunctionContext context)
    {
        _logger.LogInformation("TravelReminder triggered at: {Time}", DateTime.UtcNow);

        // TODO: find bookings where travel date is within 48 hours, send email/notification
        await Task.Delay(100); // placeholder

        _logger.LogInformation("TravelReminder completed");
    }
}
