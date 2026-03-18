using BookingService.Infrastructure.Messaging;
using Microsoft.Azure.Functions.Worker;
using Microsoft.Extensions.Logging;
using System.Text.Json;

namespace BookingService.Functions;

/// <summary>
/// Azure Function to process booking notifications from RabbitMQ.
/// </summary>
public class BookingNotificationFunction
{
    private readonly ILogger<BookingNotificationFunction> _logger;

    public BookingNotificationFunction(ILogger<BookingNotificationFunction> logger)
    {
        _logger = logger;
    }

    /// <summary>
    /// Queue-triggered function to process booking created notifications.
    /// </summary>
    [Function("ProcessBookingCreatedNotification")]
    public async Task ProcessBookingCreated(
        [QueueTrigger("booking-created-queue", Connection = "AzureWebJobsStorage")] string message)
    {
        _logger.LogInformation("Processing booking created notification: {Message}", message);

        try
        {
            var bookingEvent = JsonSerializer.Deserialize<BookingCreatedEvent>(message);
            
            if (bookingEvent == null)
            {
                _logger.LogWarning("Failed to deserialize booking created event");
                return;
            }

            // TODO: Send email notifications, SMS, push notifications, etc.
            _logger.LogInformation(
                "Booking created: AppNo={AppNo}, CreatedBy={CreatedBy}",
                bookingEvent.BookingAppNo,
                bookingEvent.CreatedBy);

            await Task.CompletedTask;
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "Error processing booking created notification");
            throw;
        }
    }

    /// <summary>
    /// Queue-triggered function to process booking status change notifications.
    /// </summary>
    [Function("ProcessBookingStatusChanged")]
    public async Task ProcessStatusChanged(
        [QueueTrigger("booking-status-changed-queue", Connection = "AzureWebJobsStorage")] string message)
    {
        _logger.LogInformation("Processing booking status changed notification: {Message}", message);

        try
        {
            var statusEvent = JsonSerializer.Deserialize<BookingStatusChangedEvent>(message);
            
            if (statusEvent == null)
            {
                _logger.LogWarning("Failed to deserialize booking status changed event");
                return;
            }

            // TODO: Send notifications based on status
            _logger.LogInformation(
                "Booking status changed: Id={Id}, AppNo={AppNo}, OldStatus={OldStatus}, NewStatus={NewStatus}",
                statusEvent.BookingId,
                statusEvent.BookingAppNo,
                statusEvent.OldStatus,
                statusEvent.NewStatus);

            await Task.CompletedTask;
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "Error processing booking status changed notification");
            throw;
        }
    }
}

// Event DTOs
internal record BookingCreatedEvent(Guid EventId, DateTime OccurredOn, string BookingAppNo, long CreatedBy);
internal record BookingStatusChangedEvent(
    Guid EventId, 
    DateTime OccurredOn, 
    long BookingId, 
    string BookingAppNo, 
    string OldStatus, 
    string NewStatus, 
    long UpdatedBy);
