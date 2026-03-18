namespace TimeAttendance.Domain.Interfaces;

public interface IUnitOfWork : IDisposable
{
    IAbsenteeismDetailRepository AbsenteeismDetails { get; }
    IAbsenteeismMisRepository AbsenteeismMis { get; }
    Task<int> SaveChangesAsync(CancellationToken cancellationToken = default);
}
