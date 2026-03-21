using MediatR;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using Microsoft.Extensions.Logging;
using BookingService.Application.Queries;

namespace BookingService.Functions;

/// <summary>
/// Azure Functions for background booking tasks.
/// In production, configure with Azure Functions SDK and triggers.
/// This implementation uses a hosted service pattern for local development.
/// </summary>
public class BookingCleanupFunction(IServiceProvider serviceProvider, ILogger<BookingCleanupFunction> logger) : BackgroundService
{
    protected override async Task ExecuteAsync(CancellationToken stoppingToken)
    {
        while (!stoppingToken.IsCancellationRequested)
        {
            try
            {
                await CleanupExpiredBookings(stoppingToken);
            }
            catch (Exception ex)
            {
                logger.LogError(ex, "Error during booking cleanup");
            }

            // Run every hour
            await Task.Delay(TimeSpan.FromHours(1), stoppingToken);
        }
    }

    private async Task CleanupExpiredBookings(CancellationToken ct)
    {
        using var scope = serviceProvider.CreateScope();
        var mediator = scope.ServiceProvider.GetRequiredService<IMediator>();

        logger.LogInformation("Running scheduled booking cleanup at {Time}", DateTime.UtcNow);

        var bookings = await mediator.Send(new GetAllBookingsQuery(), ct);
        var expiredPending = bookings
            .Where(b => b.ConfirmationStatus == "Pending" && b.LastModifiedOn < DateTime.UtcNow.AddDays(-30))
            .ToList();

        logger.LogInformation("Found {Count} expired pending bookings to review", expiredPending.Count);
    }
}

public class BookingReminderFunction(IServiceProvider serviceProvider, ILogger<BookingReminderFunction> logger) : BackgroundService
{
    protected override async Task ExecuteAsync(CancellationToken stoppingToken)
    {
        while (!stoppingToken.IsCancellationRequested)
        {
            try
            {
                await SendBookingReminders(stoppingToken);
            }
            catch (Exception ex)
            {
                logger.LogError(ex, "Error sending booking reminders");
            }

            // Run every 6 hours
            await Task.Delay(TimeSpan.FromHours(6), stoppingToken);
        }
    }

    private async Task SendBookingReminders(CancellationToken ct)
    {
        using var scope = serviceProvider.CreateScope();
        var mediator = scope.ServiceProvider.GetRequiredService<IMediator>();

        logger.LogInformation("Running booking reminder check at {Time}", DateTime.UtcNow);

        var bookings = await mediator.Send(new GetAllBookingsQuery(), ct);
        var upcomingBookings = bookings
            .Where(b => b.ConfirmationStatus == "Confirmed")
            .ToList();

        logger.LogInformation("Found {Count} upcoming confirmed bookings to send reminders for", upcomingBookings.Count);
    }
}
