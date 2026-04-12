using Microsoft.EntityFrameworkCore;
using travelTransactionService.Domain.Entities;
using travelTransactionService.Domain.Interfaces;
using travelTransactionService.Infrastructure.Data;

namespace travelTransactionService.Infrastructure.Repositories;

public class VendorMasterRepository : IVendorMasterRepository
{
    private readonly TransactionDbContext _context;

    public VendorMasterRepository(TransactionDbContext context) => _context = context;

    public async Task<VendorMaster?> GetByIdAsync(long vendorId, CancellationToken cancellationToken = default)
        => await _context.VendorMasters.FirstOrDefaultAsync(v => v.VendorId == vendorId, cancellationToken);

    public async Task<IReadOnlyList<VendorMaster>> GetAllAsync(CancellationToken cancellationToken = default)
        => await _context.VendorMasters.OrderBy(v => v.Name).ToListAsync(cancellationToken);

    public async Task<IReadOnlyList<VendorMaster>> GetByCategoryAsync(string categoryType, CancellationToken cancellationToken = default)
        => await _context.VendorMasters.Where(v => v.CategoryType == categoryType).ToListAsync(cancellationToken);

    public async Task AddAsync(VendorMaster vendor, CancellationToken cancellationToken = default)
        => await _context.VendorMasters.AddAsync(vendor, cancellationToken);

    public Task UpdateAsync(VendorMaster vendor, CancellationToken cancellationToken = default)
    {
        _context.VendorMasters.Update(vendor);
        return Task.CompletedTask;
    }

    public async Task DeleteAsync(long vendorId, CancellationToken cancellationToken = default)
    {
        var entity = await _context.VendorMasters.FirstOrDefaultAsync(v => v.VendorId == vendorId, cancellationToken);
        if (entity is not null)
            _context.VendorMasters.Remove(entity);
    }
}

public class TaxMasterRepository : ITaxMasterRepository
{
    private readonly TransactionDbContext _context;

    public TaxMasterRepository(TransactionDbContext context) => _context = context;

    public async Task<TaxMaster?> GetByTypeAsync(string taxType, CancellationToken cancellationToken = default)
        => await _context.TaxMasters.FirstOrDefaultAsync(t => t.TaxType == taxType, cancellationToken);

    public async Task<IReadOnlyList<TaxMaster>> GetAllAsync(CancellationToken cancellationToken = default)
        => await _context.TaxMasters.ToListAsync(cancellationToken);

    public async Task<IReadOnlyList<TaxMaster>> GetByVendorAsync(long vendorId, CancellationToken cancellationToken = default)
        => await _context.TaxMasters.Where(t => t.TaxVendorId == vendorId).ToListAsync(cancellationToken);

    public async Task AddAsync(TaxMaster taxMaster, CancellationToken cancellationToken = default)
        => await _context.TaxMasters.AddAsync(taxMaster, cancellationToken);

    public Task UpdateAsync(TaxMaster taxMaster, CancellationToken cancellationToken = default)
    {
        _context.TaxMasters.Update(taxMaster);
        return Task.CompletedTask;
    }
}

public class JaiInterfaceLineRepository : IJaiInterfaceLineRepository
{
    private readonly TransactionDbContext _context;

    public JaiInterfaceLineRepository(TransactionDbContext context) => _context = context;

    public async Task<JaiInterfaceLine?> GetByIdAsync(decimal interfaceLineId, CancellationToken cancellationToken = default)
        => await _context.JaiInterfaceLines
            .FirstOrDefaultAsync(l => l.InterfaceLineId == interfaceLineId, cancellationToken);

    public async Task<IReadOnlyList<JaiInterfaceLine>> GetAllAsync(CancellationToken cancellationToken = default)
        => await _context.JaiInterfaceLines
            .OrderByDescending(l => l.CreationDate).ToListAsync(cancellationToken);

    public async Task<IReadOnlyList<JaiInterfaceLine>> GetByBatchIdAsync(decimal batchId, CancellationToken cancellationToken = default)
        => await _context.JaiInterfaceLines
            .Where(l => l.BatchId == batchId).ToListAsync(cancellationToken);

    public async Task AddAsync(JaiInterfaceLine line, CancellationToken cancellationToken = default)
        => await _context.JaiInterfaceLines.AddAsync(line, cancellationToken);

    public Task UpdateAsync(JaiInterfaceLine line, CancellationToken cancellationToken = default)
    {
        _context.JaiInterfaceLines.Update(line);
        return Task.CompletedTask;
    }
}
