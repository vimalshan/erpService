using BookingService.Domain.Interfaces;
using BookingService.Infrastructure.Storage;
using Microsoft.Azure.Functions.Worker;
using Microsoft.Extensions.Logging;
using System.Text;

namespace BookingService.Functions;

/// <summary>
/// Azure Function to generate periodic booking reports.
/// </summary>
public class BookingReportFunction
{
    private readonly IBookingQueryRepository _queryRepository;
    private readonly IBlobStorageService _blobStorage;
    private readonly ILogger<BookingReportFunction> _logger;

    public BookingReportFunction(
        IBookingQueryRepository queryRepository,
        IBlobStorageService blobStorage,
        ILogger<BookingReportFunction> logger)
    {
        _queryRepository = queryRepository;
        _blobStorage = blobStorage;
        _logger = logger;
    }

    /// <summary>
    /// Timer-triggered function to generate daily booking summary report.
    /// Runs daily at 1 AM UTC.
    /// </summary>
    [Function("DailyBookingReport")]
    public async Task GenerateDailyReport([TimerTrigger("0 0 1 * * *")] TimerInfo myTimer)
    {
        _logger.LogInformation("DailyBookingReport function starting at: {Time}", DateTime.UtcNow);

        try
        {
            var yesterday = DateTime.UtcNow.Date.AddDays(-1);
            var bookings = await _queryRepository.GetAllBookingsAsync(1, 1000, statusFilter: null);

            var yesterdayBookings = bookings.Items
                .Where(b => b.CreatedOn.Date == yesterday)
                .ToList();

            // Generate CSV report
            var reportContent = GenerateCsvReport(yesterdayBookings);

            var fileName = $"booking-report-{yesterday:yyyy-MM-dd}.csv";
            var containerName = "booking-reports";

            using var stream = new MemoryStream(Encoding.UTF8.GetBytes(reportContent));
            await _blobStorage.UploadBlobAsync(containerName, fileName, stream, "text/csv");

            _logger.LogInformation(
                "Daily report generated: {FileName}, Bookings: {Count}",
                fileName,
                yesterdayBookings.Count);
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "Error generating daily booking report");
            throw;
        }
    }

    /// <summary>
    /// Timer-triggered function to generate weekly booking statistics.
    /// Runs every Monday at 2 AM UTC.
    /// </summary>
    [Function("WeeklyBookingStats")]
    public async Task GenerateWeeklyStats([TimerTrigger("0 0 2 * * MON")] TimerInfo myTimer)
    {
        _logger.LogInformation("WeeklyBookingStats function starting at: {Time}", DateTime.UtcNow);

        try
        {
            var lastWeekStart = DateTime.UtcNow.Date.AddDays(-7);
            var lastWeekEnd = DateTime.UtcNow.Date.AddDays(-1);

            var bookings = await _queryRepository.GetAllBookingsAsync(1, 10000, statusFilter: null);

            var weeklyBookings = bookings.Items
                .Where(b => b.CreatedOn.Date >= lastWeekStart && b.CreatedOn.Date <= lastWeekEnd)
                .ToList();

            var stats = new
            {
                Week = $"{lastWeekStart:yyyy-MM-dd} to {lastWeekEnd:yyyy-MM-dd}",
                TotalBookings = weeklyBookings.Count,
                ByStatus = weeklyBookings.GroupBy(b => b.Status)
                    .Select(g => new { Status = g.Key, Count = g.Count() })
                    .ToList(),
                TopLocations = weeklyBookings
                    .Where(b => !string.IsNullOrEmpty(b.LocationCode))
                    .GroupBy(b => b.LocationCode)
                    .Select(g => new { Location = g.Key, Count = g.Count() })
                    .OrderByDescending(x => x.Count)
                    .Take(5)
                    .ToList()
            };

            var reportContent = System.Text.Json.JsonSerializer.Serialize(stats, new System.Text.Json.JsonSerializerOptions 
            { 
                WriteIndented = true 
            });

            var fileName = $"booking-stats-{lastWeekStart:yyyy-MM-dd}.json";
            var containerName = "booking-reports";

            using var stream = new MemoryStream(Encoding.UTF8.GetBytes(reportContent));
            await _blobStorage.UploadBlobAsync(containerName, fileName, stream, "application/json");

            _logger.LogInformation("Weekly stats generated: {FileName}", fileName);
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "Error generating weekly booking stats");
            throw;
        }
    }

    private string GenerateCsvReport(IEnumerable<dynamic> bookings)
    {
        var sb = new StringBuilder();
        sb.AppendLine("BookingId,AppNo,Title,LocationCode,BookingDate,Status,CreatedOn");

        foreach (var booking in bookings)
        {
            sb.AppendLine($"{booking.BookingId},{booking.BookingAppNo},{booking.BookingTitle}," +
                         $"{booking.LocationCode},{booking.BookingDate:yyyy-MM-dd},{booking.Status},{booking.CreatedOn:yyyy-MM-dd HH:mm:ss}");
        }

        return sb.ToString();
    }
}
