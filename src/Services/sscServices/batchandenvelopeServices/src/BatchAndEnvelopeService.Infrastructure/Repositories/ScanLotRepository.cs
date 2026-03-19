using Microsoft.EntityFrameworkCore;
using BatchAndEnvelopeService.Domain.Entities;
using BatchAndEnvelopeService.Domain.Interfaces;
using BatchAndEnvelopeService.Infrastructure.Persistence;

namespace BatchAndEnvelopeService.Infrastructure.Repositories;

public class ScanLotRepository : IScanLotRepository
{
    private readonly ApplicationDbContext _context;

    public ScanLotRepository(ApplicationDbContext context) => _context = context;

    public async Task<ScanLotMaster?> GetByIdAsync(long lotNo, CancellationToken ct = default)
        => await _context.ScanLotMasters.FindAsync(new object[] { lotNo }, ct);

    public async Task<IEnumerable<ScanLotMaster>> GetAllAsync(CancellationToken ct = default)
        => await _context.ScanLotMasters.OrderByDescending(s => s.CreatedOn).ToListAsync(ct);

    public async Task AddAsync(ScanLotMaster scanLot, CancellationToken ct = default)
        => await _context.ScanLotMasters.AddAsync(scanLot, ct);

    public Task UpdateAsync(ScanLotMaster scanLot, CancellationToken ct = default)
    {
        _context.ScanLotMasters.Update(scanLot);
        return Task.CompletedTask;
    }
}
