using MeetingModule.Domain.Interfaces;
using Microsoft.Azure.Functions.Worker;
using Microsoft.Extensions.Logging;

namespace MeetingModule.Functions;

public class MeetingCleanupFunction(IUnitOfWork uow, ILogger<MeetingCleanupFunction> logger)
{
    /// <summary>
    /// Runs daily at midnight to archive old completed meetings.
    /// </summary>
    [Function("MeetingCleanup")]
    public async Task Run([TimerTrigger("0 0 0 * * *")] TimerInfo timer)
    {
        logger.LogInformation("MeetingCleanup function started at {Time}", DateTime.UtcNow);

        var cutoffDate = DateTime.UtcNow.AddDays(-90);
        var oldMeetings = await uow.MeetingSchedules.GetByDateRangeAsync(DateTime.MinValue, cutoffDate);

        var archiveCount = 0;
        foreach (var meeting in oldMeetings.Where(m => m.MeetingStatus == "COMPLETED"))
        {
            // Close open polls
            var polls = await uow.PollDetails.GetByMeetingIdAsync(meeting.MeetingId);
            foreach (var poll in polls.Where(p => p.PollStatus == "ACTIVE"))
            {
                poll.Archive(null);
                await uow.PollDetails.UpdateAsync(poll);
            }
            archiveCount++;
        }

        if (archiveCount > 0)
            await uow.SaveChangesAsync();

        logger.LogInformation("MeetingCleanup archived polls for {Count} meetings", archiveCount);
    }
}
