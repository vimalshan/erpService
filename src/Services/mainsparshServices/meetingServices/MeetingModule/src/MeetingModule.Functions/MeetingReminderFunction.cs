using MeetingModule.Domain.Interfaces;
using Microsoft.Azure.Functions.Worker;
using Microsoft.Azure.Functions.Worker.Http;
using Microsoft.Extensions.Logging;

namespace MeetingModule.Functions;

public class MeetingReminderFunction(IUnitOfWork uow, ILogger<MeetingReminderFunction> logger)
{
    /// <summary>
    /// Runs every hour to send reminders for upcoming meetings.
    /// </summary>
    [Function("MeetingReminder")]
    public async Task RunTimer([TimerTrigger("0 0 * * * *")] TimerInfo timer)
    {
        logger.LogInformation("MeetingReminder function started at {Time}", DateTime.UtcNow);

        var upcoming = await uow.MeetingSchedules.GetByDateRangeAsync(
            DateTime.UtcNow,
            DateTime.UtcNow.AddHours(1));

        foreach (var meeting in upcoming.Where(m => m.MeetingStatus == "SCHEDULED"))
        {
            logger.LogInformation("Reminder: Meeting '{Title}' starts at {Date}",
                meeting.MeetingTitle, meeting.MeetingDate);

            // In production: send email/notification via a notification service
        }
    }

    /// <summary>
    /// HTTP-triggered function to manually check meeting status.
    /// </summary>
    [Function("MeetingStatusCheck")]
    public async Task<HttpResponseData> RunHttp(
        [HttpTrigger(AuthorizationLevel.Function, "get")] HttpRequestData req)
    {
        logger.LogInformation("MeetingStatusCheck HTTP trigger invoked");

        var scheduled = await uow.MeetingSchedules.GetByStatusAsync("SCHEDULED");
        var ongoing = await uow.MeetingSchedules.GetByStatusAsync("ONGOING");

        var response = req.CreateResponse(System.Net.HttpStatusCode.OK);
        await response.WriteAsJsonAsync(new
        {
            ScheduledCount = scheduled.Count,
            OngoingCount = ongoing.Count,
            CheckedAt = DateTime.UtcNow
        });

        return response;
    }
}
