using BookingService.Domain.Interfaces;
using BookingService.Domain.ValueObjects;
using Microsoft.Azure.Functions.Worker;
using Microsoft.Extensions.Logging;

namespace BookingService.Functions;

/// <summary>
/// Azure Function to cleanup expired draft bookings.
/// Runs daily at midnight UTC.
/// </summary>
public class BookingCleanupFunction
{
    private readonly IBookingRepository _repository;
    private readonly IUnitOfWork _unitOfWork;
    private readonly ILogger<BookingCleanupFunction> _logger;

    public BookingCleanupFunction(
        IBookingRepository repository,
        IUnitOfWork unitOfWork,
        ILogger<BookingCleanupFunction> logger)
    {
        _repository = repository;
        _unitOfWork = unitOfWork;
        _logger = logger;
    }

    /// <summary>
    /// Timer-triggered function to cancel bookings in DRAFT status older than 30 days.
    /// </summary>
    [Function("BookingCleanup")]
    public async Task Run([TimerTrigger("0 0 0 * * *")] TimerInfo myTimer)
    {
        _logger.LogInformation("BookingCleanup function starting at: {Time}", DateTime.UtcNow);

        try
        {
            var cutoffDate = DateTime.UtcNow.AddDays(-30);
            var expiredDrafts = await _repository.GetBookingsByStatusAsync(BookingStatus.Draft);

            var toCancel = expiredDrafts
                .Where(b => b.CreatedOn < cutoffDate)
                .ToList();

            _logger.LogInformation("Found {Count} expired draft bookings to cancel", toCancel.Count);

            foreach (var booking in toCancel)
            {
                booking.Cancel(updatedBy: 0); // System user
                await _repository.UpdateAsync(booking);
            }

            await _unitOfWork.SaveChangesAsync();

            _logger.LogInformation("Successfully cancelled {Count} expired draft bookings", toCancel.Count);
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "Error in BookingCleanup function");
            throw;
        }
    }
}
