using MeetingModule.Domain.Entities;
using MeetingModule.Domain.Interfaces;
using Microsoft.EntityFrameworkCore;

namespace MeetingModule.Infrastructure.Persistence.Repositories;

public class MeetingTypeRepository(MeetingDbContext context) : IMeetingTypeRepository
{
    public async Task<MeetingType?> GetByIdAsync(long id, CancellationToken ct = default) =>
        await context.MeetingTypes.FindAsync([id], ct);

    public async Task<MeetingType?> GetByCodeAsync(string code, CancellationToken ct = default) =>
        await context.MeetingTypes.FirstOrDefaultAsync(x => x.MeetTypeCode == code, ct);

    public async Task<IReadOnlyList<MeetingType>> GetAllAsync(CancellationToken ct = default) =>
        await context.MeetingTypes.OrderBy(x => x.MeetTypeName).ToListAsync(ct);

    public async Task<IReadOnlyList<MeetingType>> GetActiveAsync(CancellationToken ct = default) =>
        await context.MeetingTypes.Where(x => x.MeetTypeStatus == "A").OrderBy(x => x.MeetTypeName).ToListAsync(ct);

    public async Task<MeetingType> AddAsync(MeetingType entity, CancellationToken ct = default)
    {
        await context.MeetingTypes.AddAsync(entity, ct);
        return entity;
    }

    public Task UpdateAsync(MeetingType entity, CancellationToken ct = default)
    {
        context.MeetingTypes.Update(entity);
        return Task.CompletedTask;
    }
}

public class MeetingScheduleRepository(MeetingDbContext context) : IMeetingScheduleRepository
{
    public async Task<MeetingSchedule?> GetByIdAsync(long id, CancellationToken ct = default) =>
        await context.MeetingSchedules.Include(m => m.MeetingType).FirstOrDefaultAsync(m => m.MeetingId == id, ct);

    public async Task<MeetingSchedule?> GetByIdWithPollsAsync(long id, CancellationToken ct = default) =>
        await context.MeetingSchedules
            .Include(m => m.MeetingType)
            .Include(m => m.Polls)
            .FirstOrDefaultAsync(m => m.MeetingId == id, ct);

    public async Task<IReadOnlyList<MeetingSchedule>> GetAllAsync(CancellationToken ct = default) =>
        await context.MeetingSchedules
            .Include(m => m.MeetingType)
            .OrderByDescending(m => m.MeetingDate)
            .ToListAsync(ct);

    public async Task<IReadOnlyList<MeetingSchedule>> GetByDateRangeAsync(DateTime from, DateTime to, CancellationToken ct = default) =>
        await context.MeetingSchedules
            .Include(m => m.MeetingType)
            .Where(m => m.MeetingDate >= from && m.MeetingDate <= to)
            .OrderBy(m => m.MeetingDate)
            .ToListAsync(ct);

    public async Task<IReadOnlyList<MeetingSchedule>> GetByStatusAsync(string status, CancellationToken ct = default) =>
        await context.MeetingSchedules
            .Include(m => m.MeetingType)
            .Where(m => m.MeetingStatus == status)
            .OrderByDescending(m => m.MeetingDate)
            .ToListAsync(ct);

    public async Task<IReadOnlyList<MeetingSchedule>> GetByOrganizerAsync(long organizerId, CancellationToken ct = default) =>
        await context.MeetingSchedules
            .Include(m => m.MeetingType)
            .Where(m => m.OrganizerId == organizerId)
            .OrderByDescending(m => m.MeetingDate)
            .ToListAsync(ct);

    public async Task<MeetingSchedule> AddAsync(MeetingSchedule entity, CancellationToken ct = default)
    {
        await context.MeetingSchedules.AddAsync(entity, ct);
        return entity;
    }

    public Task UpdateAsync(MeetingSchedule entity, CancellationToken ct = default)
    {
        context.MeetingSchedules.Update(entity);
        return Task.CompletedTask;
    }
}

public class PollDetailRepository(MeetingDbContext context) : IPollDetailRepository
{
    public async Task<PollDetail?> GetByIdAsync(long id, CancellationToken ct = default) =>
        await context.PollDetails.FindAsync([id], ct);

    public async Task<IReadOnlyList<PollDetail>> GetByMeetingIdAsync(long meetingId, CancellationToken ct = default) =>
        await context.PollDetails.Where(p => p.MeetingId == meetingId).ToListAsync(ct);

    public async Task<PollDetail> AddAsync(PollDetail entity, CancellationToken ct = default)
    {
        await context.PollDetails.AddAsync(entity, ct);
        return entity;
    }

    public Task UpdateAsync(PollDetail entity, CancellationToken ct = default)
    {
        context.PollDetails.Update(entity);
        return Task.CompletedTask;
    }
}
