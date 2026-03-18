using MeetingModule.Domain.Interfaces;

namespace MeetingModule.Infrastructure.Persistence.Repositories;

public class UnitOfWork(MeetingDbContext context,
    IMeetingTypeRepository meetingTypeRepository,
    IMeetingScheduleRepository meetingScheduleRepository,
    IPollDetailRepository pollDetailRepository) : IUnitOfWork
{
    public IMeetingTypeRepository MeetingTypes => meetingTypeRepository;
    public IMeetingScheduleRepository MeetingSchedules => meetingScheduleRepository;
    public IPollDetailRepository PollDetails => pollDetailRepository;

    public async Task<int> SaveChangesAsync(CancellationToken ct = default)
        => await context.SaveChangesAsync(ct);

    public void Dispose()
    {
        context.Dispose();
        GC.SuppressFinalize(this);
    }
}
