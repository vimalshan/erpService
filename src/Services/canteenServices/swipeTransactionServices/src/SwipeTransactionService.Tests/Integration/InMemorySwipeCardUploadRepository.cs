using SwipeTransactionService.Domain.Entities;
using SwipeTransactionService.Domain.Interfaces.Repositories;

namespace SwipeTransactionService.Tests.Integration;

/// <summary>
/// In-memory stub for <see cref="ISwipeCardUploadRepository"/> used in integration tests.
/// The real EF repository uses a HasNoKey entity which EF InMemory cannot track.
/// This stub stores entities in a plain list without EF change tracking.
/// </summary>
internal sealed class InMemorySwipeCardUploadRepository : ISwipeCardUploadRepository
{
    private readonly List<SwipeCardUpload> _store = new();
    private readonly Lock _lock = new();

    public Task<SwipeCardUpload?> GetBySerialNumberAsync(long serialNumber, CancellationToken ct = default)
    {
        lock (_lock)
        {
            var entity = _store.FirstOrDefault(x => x.SerialNumber == serialNumber);
            return Task.FromResult(entity);
        }
    }

    public Task<IEnumerable<SwipeCardUpload>> GetByEmployeeAsync(
        string employeeNumber, DateTime from, DateTime to, CancellationToken ct = default)
    {
        lock (_lock)
        {
            var result = _store
                .Where(x => x.EmployeeNumber == employeeNumber &&
                            x.SwipeTime >= from &&
                            x.SwipeTime <= to)
                .ToList()
                .AsEnumerable();
            return Task.FromResult(result);
        }
    }

    public Task<IEnumerable<SwipeCardUpload>> GetPendingAsync(CancellationToken ct = default)
    {
        lock (_lock)
        {
            var result = _store.Where(x => x.UpdateStatus == 'P').ToList().AsEnumerable();
            return Task.FromResult(result);
        }
    }

    public Task AddAsync(SwipeCardUpload entity, CancellationToken ct = default)
    {
        lock (_lock)
        {
            _store.Add(entity);
        }
        return Task.CompletedTask;
    }

    public Task UpdateAsync(SwipeCardUpload entity, CancellationToken ct = default)
    {
        lock (_lock)
        {
            var index = _store.FindIndex(x => x.SerialNumber == entity.SerialNumber);
            if (index >= 0) _store[index] = entity;
        }
        return Task.CompletedTask;
    }

    public Task<long> GetNextSerialNumberAsync(CancellationToken ct = default)
    {
        lock (_lock)
        {
            var max = _store.Count == 0 ? 0L : _store.Max(x => x.SerialNumber);
            return Task.FromResult(max + 1);
        }
    }
}
