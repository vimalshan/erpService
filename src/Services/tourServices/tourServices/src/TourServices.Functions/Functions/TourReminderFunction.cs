using Microsoft.Azure.Functions.Worker;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging;
using TourServices.Infrastructure.Persistence;

namespace TourServices.Functions.Functions;

/// <summary>
/// Sends reminder notifications to participants 24 hours before a tour starts.
/// Schedule: Every day at 8:00 AM UTC
/// </summary>
public sealed class TourReminderFunction
{
    private readonly ApplicationDbContext _context;
    private readonly ILogger<TourReminderFunction> _logger;

    public TourReminderFunction(ApplicationDbContext context, ILogger<TourReminderFunction> logger)
    {
        _context = context;
        _logger = logger;
    }

    [Function(nameof(TourReminderFunction))]
    public async Task Run([TimerTrigger("0 0 8 * * *")] TimerInfo timerInfo, CancellationToken ct)
    {
        _logger.LogInformation("Tour reminder function started at {Time}", DateTime.UtcNow);

        var tomorrow = DateOnly.FromDateTime(DateTime.UtcNow.AddDays(1));

        var tomorrowsTours = await _context.TourPackages
            .Include(t => t.Registrations)
            .Where(t => t.StartDate == tomorrow &&
                        EF.Property<string>(t, "TOUR_STATUS") == "A")
            .ToListAsync(ct);

        foreach (var tour in tomorrowsTours)
        {
            var activeRegistrations = tour.Registrations
                .Where(r => r.RegistrationStatus == Domain.ValueObjects.RegistrationStatus.Active)
                .ToList();

            _logger.LogInformation(
                "Sending reminders for tour {TourId} ({TourName}) to {Count} participants",
                tour.TourId, tour.TourName, activeRegistrations.Count);

            foreach (var reg in activeRegistrations)
            {
                // In a real scenario, dispatch an email/notification to ParticipantId
                _logger.LogInformation(
                    "  → Reminder queued for ParticipantId={ParticipantId}", reg.ParticipantId);
            }
        }

        _logger.LogInformation("Tour reminder function finished. {Count} tours processed.", tomorrowsTours.Count);
    }
}
