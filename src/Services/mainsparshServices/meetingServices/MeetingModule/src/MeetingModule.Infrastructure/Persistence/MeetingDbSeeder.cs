using MeetingModule.Domain.Entities;
using Microsoft.EntityFrameworkCore;

namespace MeetingModule.Infrastructure.Persistence;

public static class MeetingDbSeeder
{
    public static async Task SeedAsync(MeetingDbContext context)
    {
        await context.Database.MigrateAsync();

        if (await context.MeetingTypes.AnyAsync())
            return;

        var meetingTypes = new[]
        {
            MeetingType.Create("BOARD", "Board Meeting", "Regular board of directors meeting", 1),
            MeetingType.Create("STANDUP", "Daily Standup", "Daily team standup meeting", 1),
            MeetingType.Create("REVIEW", "Sprint Review", "Sprint review and retrospective", 1),
            MeetingType.Create("TOWNHALL", "Town Hall", "Company-wide town hall meeting", 1),
            MeetingType.Create("INTERVIEW", "Interview", "Candidate interview session", 1),
            MeetingType.Create("TRAINING", "Training Session", "Employee training and development", 1),
        };

        context.MeetingTypes.AddRange(meetingTypes);
        await context.SaveChangesAsync();

        // Seed sample meetings
        var meetings = new[]
        {
            MeetingSchedule.Create(meetingTypes[0].MeetTypeId, "Q1 Board Review", DateTime.UtcNow.AddDays(7),
                "Conference Room A", 120, 1, "Quarterly board review meeting", 1),
            MeetingSchedule.Create(meetingTypes[1].MeetTypeId, "Team Alpha Standup", DateTime.UtcNow.AddDays(1),
                "Virtual - Teams", 15, 2, "Daily standup for Team Alpha", 1),
            MeetingSchedule.Create(meetingTypes[2].MeetTypeId, "Sprint 24 Review", DateTime.UtcNow.AddDays(14),
                "Conference Room B", 60, 3, "Sprint 24 review and demo", 1),
        };

        context.MeetingSchedules.AddRange(meetings);
        await context.SaveChangesAsync();
    }
}
