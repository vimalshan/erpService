using GSTComplianceService.Domain.Entities;
using GSTComplianceService.Domain.Interfaces;
using GSTComplianceService.Infrastructure.Persistence;
using Microsoft.EntityFrameworkCore;

namespace GSTComplianceService.Infrastructure.Repositories;

public class GstMainRepository : IGstMainRepository
{
    private readonly GstDbContext _context;

    public GstMainRepository(GstDbContext context) => _context = context;

    public async Task<GstMain?> GetByIdAsync(long gstId, CancellationToken cancellationToken = default)
        => await _context.GstMains
            .Include(g => g.HsnDetails)
            .Include(g => g.ServiceDetails)
            .Include(g => g.StateRegDetails)
            .FirstOrDefaultAsync(g => g.GstId == gstId, cancellationToken);

    public async Task<GstMain?> GetByPanNoAsync(string panNo, CancellationToken cancellationToken = default)
        => await _context.GstMains
            .Include(g => g.HsnDetails)
            .Include(g => g.ServiceDetails)
            .Include(g => g.StateRegDetails)
            .FirstOrDefaultAsync(g => g.GstPanNo == panNo, cancellationToken);

    public async Task<IEnumerable<GstMain>> GetAllAsync(int page, int pageSize, CancellationToken cancellationToken = default)
        => await _context.GstMains
            .OrderByDescending(g => g.GstCreatedOn)
            .Skip((page - 1) * pageSize)
            .Take(pageSize)
            .ToListAsync(cancellationToken);

    public async Task<long> GetTotalCountAsync(CancellationToken cancellationToken = default)
        => await _context.GstMains.LongCountAsync(cancellationToken);

    public async Task<GstMain> AddAsync(GstMain gstMain, CancellationToken cancellationToken = default)
    {
        await _context.GstMains.AddAsync(gstMain, cancellationToken);
        return gstMain;
    }

    public Task UpdateAsync(GstMain gstMain, CancellationToken cancellationToken = default)
    {
        _context.GstMains.Update(gstMain);
        return Task.CompletedTask;
    }

    public async Task DeleteAsync(long gstId, CancellationToken cancellationToken = default)
    {
        var entity = await _context.GstMains.FindAsync(new object[] { gstId }, cancellationToken);
        if (entity is not null)
            _context.GstMains.Remove(entity);
    }

    public async Task<bool> ExistsByPanNoAsync(string panNo, CancellationToken cancellationToken = default)
        => await _context.GstMains.AnyAsync(g => g.GstPanNo == panNo, cancellationToken);
}

public class GstHsnDetailRepository : IGstHsnDetailRepository
{
    private readonly GstDbContext _context;

    public GstHsnDetailRepository(GstDbContext context) => _context = context;

    public async Task<IEnumerable<GstHsnDetail>> GetByGstIdAsync(long gstId, CancellationToken cancellationToken = default)
        => await _context.GstHsnDetails.Where(h => h.GstHsnGstId == gstId).ToListAsync(cancellationToken);

    public async Task<GstHsnDetail?> GetByIdAsync(long hsnId, CancellationToken cancellationToken = default)
        => await _context.GstHsnDetails.FindAsync(new object[] { hsnId }, cancellationToken);

    public async Task<GstHsnDetail> AddAsync(GstHsnDetail detail, CancellationToken cancellationToken = default)
    {
        await _context.GstHsnDetails.AddAsync(detail, cancellationToken);
        return detail;
    }

    public Task UpdateAsync(GstHsnDetail detail, CancellationToken cancellationToken = default)
    {
        _context.GstHsnDetails.Update(detail);
        return Task.CompletedTask;
    }

    public async Task DeleteAsync(long hsnId, CancellationToken cancellationToken = default)
    {
        var entity = await _context.GstHsnDetails.FindAsync(new object[] { hsnId }, cancellationToken);
        if (entity is not null)
            _context.GstHsnDetails.Remove(entity);
    }
}

public class GstStateRegDetailRepository : IGstStateRegDetailRepository
{
    private readonly GstDbContext _context;

    public GstStateRegDetailRepository(GstDbContext context) => _context = context;

    public async Task<IEnumerable<GstStateRegDetail>> GetByGstIdAsync(long gstId, CancellationToken cancellationToken = default)
        => await _context.GstStateRegDetails.Where(s => s.GstId == gstId).ToListAsync(cancellationToken);

    public async Task<GstStateRegDetail?> GetByIdAsync(long tinId, CancellationToken cancellationToken = default)
        => await _context.GstStateRegDetails.FindAsync(new object[] { tinId }, cancellationToken);

    public async Task<GstStateRegDetail> AddAsync(GstStateRegDetail detail, CancellationToken cancellationToken = default)
    {
        await _context.GstStateRegDetails.AddAsync(detail, cancellationToken);
        return detail;
    }

    public Task UpdateAsync(GstStateRegDetail detail, CancellationToken cancellationToken = default)
    {
        _context.GstStateRegDetails.Update(detail);
        return Task.CompletedTask;
    }

    public async Task DeleteAsync(long tinId, CancellationToken cancellationToken = default)
    {
        var entity = await _context.GstStateRegDetails.FindAsync(new object[] { tinId }, cancellationToken);
        if (entity is not null)
            _context.GstStateRegDetails.Remove(entity);
    }
}
