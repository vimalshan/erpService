using Microsoft.EntityFrameworkCore;
using VehicleTracking.Domain.Entities;
using VehicleTracking.Domain.Interfaces;
using VehicleTracking.Infrastructure.Persistence;

namespace VehicleTracking.Infrastructure.Repositories;

public class Repository<T>(VehicleTrackingDbContext context) : IRepository<T> where T : class
{
    protected readonly VehicleTrackingDbContext Context = context;

    public virtual async Task<T?> GetByIdAsync(long id, CancellationToken ct = default)
        => await Context.Set<T>().FindAsync([id], ct);

    public virtual async Task<IEnumerable<T>> GetAllAsync(CancellationToken ct = default)
        => await Context.Set<T>().ToListAsync(ct);

    public virtual async Task<T> AddAsync(T entity, CancellationToken ct = default)
    {
        await Context.Set<T>().AddAsync(entity, ct);
        return entity;
    }

    public virtual Task UpdateAsync(T entity, CancellationToken ct = default)
    {
        Context.Set<T>().Update(entity);
        return Task.CompletedTask;
    }

    public virtual Task DeleteAsync(T entity, CancellationToken ct = default)
    {
        Context.Set<T>().Remove(entity);
        return Task.CompletedTask;
    }
}

public class VehicleMasterRepository(VehicleTrackingDbContext context)
    : Repository<VehicleMaster>(context), IVehicleMasterRepository
{
    public async Task<VehicleMaster?> GetByRegistrationAsync(string regNum1, string regNum4, CancellationToken ct = default)
        => await Context.VehicleMasters.FirstOrDefaultAsync(v => v.RegNum1 == regNum1 && v.RegNum4 == regNum4, ct);
}

public class VehicleStageRepository(VehicleTrackingDbContext context)
    : Repository<VehicleStage>(context), IVehicleStageRepository
{
    public async Task<IEnumerable<VehicleStage>> GetByTrackingNumberAsync(long trackingNumber, CancellationToken ct = default)
        => await Context.VehicleStages
            .Include(s => s.Stage)
            .Where(s => s.TransactionNumber == trackingNumber)
            .OrderByDescending(s => s.EntryDate)
            .ToListAsync(ct);
}

public class VehicleTransactionRepository(VehicleTrackingDbContext context)
    : Repository<VehicleTransaction>(context), IVehicleTransactionRepository
{
    public async Task<IEnumerable<VehicleTransaction>> GetByTrackingNumberAsync(long trackingNumber, CancellationToken ct = default)
        => await Context.VehicleTransactions
            .Where(t => t.TrackingNumber == trackingNumber)
            .ToListAsync(ct);

    public async Task<IEnumerable<VehicleTransaction>> GetActiveTransactionsAsync(CancellationToken ct = default)
        => await Context.VehicleTransactions
            .Where(t => t.VehicleStatus == 'A')
            .ToListAsync(ct);
}

public class VehicleInvoiceRepository(VehicleTrackingDbContext context)
    : Repository<VehicleInvoice>(context), IVehicleInvoiceRepository
{
    public async Task<IEnumerable<VehicleInvoice>> GetByTrackingNumberAsync(long trackingNumber, CancellationToken ct = default)
        => await Context.VehicleInvoices
            .Where(i => i.TrackingNumber == trackingNumber)
            .ToListAsync(ct);
}

public class StageMasterRepository(VehicleTrackingDbContext context)
    : Repository<StageMaster>(context), IStageMasterRepository;

public class PurposeMasterRepository(VehicleTrackingDbContext context)
    : Repository<PurposeMaster>(context), IPurposeMasterRepository
{
    public async Task<PurposeMaster?> GetWithStagesAsync(long purposeCode, CancellationToken ct = default)
        => await Context.PurposeMasters
            .Include(p => p.PurposeStages)
                .ThenInclude(ps => ps.Stage)
            .FirstOrDefaultAsync(p => p.PurposeCode == purposeCode, ct);
}

public class DecisionFlagRepository(VehicleTrackingDbContext context)
    : Repository<DecisionFlag>(context), IDecisionFlagRepository
{
    public async Task<IEnumerable<DecisionFlag>> GetByTrackingNumberAsync(long trackingNumber, CancellationToken ct = default)
        => await Context.DecisionFlags
            .Where(d => d.TrackingNumber == trackingNumber)
            .ToListAsync(ct);
}

public class WeightInfoRepository(VehicleTrackingDbContext context)
    : Repository<WeightInformation>(context), IWeightInfoRepository
{
    public async Task<WeightInformation?> GetByTrackingNumberAsync(long trackingNumber, CancellationToken ct = default)
        => await Context.WeightInformations
            .FirstOrDefaultAsync(w => w.TrackingNumber == trackingNumber, ct);
}
