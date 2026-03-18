using Microsoft.EntityFrameworkCore;
using OtherService.Domain.Entities;
using OtherService.Domain.Interfaces;
using OtherService.Infrastructure.Persistence;

namespace OtherService.Infrastructure.Repositories;

public sealed class LogDdCatDevDetailRepository : ILogDdCatDevDetailRepository
{
    private readonly OtherDbContext _context;

    public LogDdCatDevDetailRepository(OtherDbContext context) => _context = context;

    public async Task<IEnumerable<LogDdCatDevDetail>> GetAllAsync(CancellationToken ct = default) =>
        await _context.LogDdCatDevDetails.AsNoTracking().ToListAsync(ct);

    public async Task<LogDdCatDevDetail?> GetByKeyAsync(
        string appId, decimal appNum, CancellationToken ct = default) =>
        await _context.LogDdCatDevDetails
            .FirstOrDefaultAsync(e => e.AppId == appId && e.AppNum == appNum, ct);

    public async Task<IEnumerable<LogDdCatDevDetail>> GetByReqNumAsync(
        decimal reqNum, CancellationToken ct = default) =>
        await _context.LogDdCatDevDetails
            .AsNoTracking()
            .Where(e => e.ReqNum == reqNum)
            .ToListAsync(ct);

    public async Task AddAsync(LogDdCatDevDetail entity, CancellationToken ct = default) =>
        await _context.LogDdCatDevDetails.AddAsync(entity, ct);

    public void Update(LogDdCatDevDetail entity) =>
        _context.LogDdCatDevDetails.Update(entity);

    public void Delete(LogDdCatDevDetail entity) =>
        _context.LogDdCatDevDetails.Remove(entity);

    public Task<int> SaveChangesAsync(CancellationToken ct = default) =>
        _context.SaveChangesAsync(ct);
}
