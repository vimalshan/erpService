using TimeAttendance.Domain.Entities;

namespace TimeAttendance.Domain.Interfaces;

public interface IAbsenteeismDetailRepository
{
    Task<AbsenteeismDetail?> GetByIdAsync(long id, CancellationToken cancellationToken = default);
    Task<IEnumerable<AbsenteeismDetail>> GetAllAsync(CancellationToken cancellationToken = default);
    Task<IEnumerable<AbsenteeismDetail>> GetByUnitAndPeriodAsync(long unitId, int year, int month, CancellationToken cancellationToken = default);
    Task<IEnumerable<AbsenteeismDetail>> GetByUnitAsync(long unitId, CancellationToken cancellationToken = default);
    Task AddAsync(AbsenteeismDetail entity, CancellationToken cancellationToken = default);
    void Update(AbsenteeismDetail entity);
    void Remove(AbsenteeismDetail entity);
    Task<int> SaveChangesAsync(CancellationToken cancellationToken = default);
}

public interface IAbsenteeismMisRepository
{
    Task<AbsenteeismMis?> GetByIdAsync(long id, CancellationToken cancellationToken = default);
    Task<IEnumerable<AbsenteeismMis>> GetAllAsync(CancellationToken cancellationToken = default);
    Task<IEnumerable<AbsenteeismMis>> GetByUnitAndMonthAsync(int unitId, string month, CancellationToken cancellationToken = default);
    Task AddAsync(AbsenteeismMis entity, CancellationToken cancellationToken = default);
    void Update(AbsenteeismMis entity);
    void Remove(AbsenteeismMis entity);
    Task<int> SaveChangesAsync(CancellationToken cancellationToken = default);
}
