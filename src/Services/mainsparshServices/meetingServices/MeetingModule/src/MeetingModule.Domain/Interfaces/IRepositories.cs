using MeetingModule.Domain.Entities;

namespace MeetingModule.Domain.Interfaces;

public interface IMeetingTypeRepository
{
    Task<MeetingType?> GetByIdAsync(long id, CancellationToken ct = default);
    Task<MeetingType?> GetByCodeAsync(string code, CancellationToken ct = default);
    Task<IReadOnlyList<MeetingType>> GetAllAsync(CancellationToken ct = default);
    Task<IReadOnlyList<MeetingType>> GetActiveAsync(CancellationToken ct = default);
    Task<MeetingType> AddAsync(MeetingType entity, CancellationToken ct = default);
    Task UpdateAsync(MeetingType entity, CancellationToken ct = default);
}

public interface IMeetingScheduleRepository
{
    Task<MeetingSchedule?> GetByIdAsync(long id, CancellationToken ct = default);
    Task<MeetingSchedule?> GetByIdWithPollsAsync(long id, CancellationToken ct = default);
    Task<IReadOnlyList<MeetingSchedule>> GetAllAsync(CancellationToken ct = default);
    Task<IReadOnlyList<MeetingSchedule>> GetByDateRangeAsync(DateTime from, DateTime to, CancellationToken ct = default);
    Task<IReadOnlyList<MeetingSchedule>> GetByStatusAsync(string status, CancellationToken ct = default);
    Task<IReadOnlyList<MeetingSchedule>> GetByOrganizerAsync(long organizerId, CancellationToken ct = default);
    Task<MeetingSchedule> AddAsync(MeetingSchedule entity, CancellationToken ct = default);
    Task UpdateAsync(MeetingSchedule entity, CancellationToken ct = default);
}

public interface IPollDetailRepository
{
    Task<PollDetail?> GetByIdAsync(long id, CancellationToken ct = default);
    Task<IReadOnlyList<PollDetail>> GetByMeetingIdAsync(long meetingId, CancellationToken ct = default);
    Task<PollDetail> AddAsync(PollDetail entity, CancellationToken ct = default);
    Task UpdateAsync(PollDetail entity, CancellationToken ct = default);
}

public interface IUnitOfWork : IDisposable
{
    IMeetingTypeRepository MeetingTypes { get; }
    IMeetingScheduleRepository MeetingSchedules { get; }
    IPollDetailRepository PollDetails { get; }
    Task<int> SaveChangesAsync(CancellationToken ct = default);
}
