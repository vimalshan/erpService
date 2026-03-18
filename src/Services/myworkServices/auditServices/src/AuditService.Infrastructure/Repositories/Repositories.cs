using AuditService.Domain.Common;
using AuditService.Domain.Entities;
using AuditService.Domain.Interfaces;
using AuditService.Infrastructure.Data;
using MediatR;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Storage;

namespace AuditService.Infrastructure.Repositories;

public class AuditRepository : IAuditRepository
{
    private readonly AuditDbContext _context;

    public AuditRepository(AuditDbContext context) => _context = context;

    public async Task<AuditMaster?> GetByIdAsync(long auditId, CancellationToken cancellationToken = default)
        => await _context.AuditMasters
            .Include(a => a.Observations)
            .FirstOrDefaultAsync(a => a.AuditId == auditId, cancellationToken);

    public async Task<IEnumerable<AuditMaster>> GetAllAsync(CancellationToken cancellationToken = default)
        => await _context.AuditMasters
            .Include(a => a.Observations)
            .ToListAsync(cancellationToken);

    public async Task<IEnumerable<AuditMaster>> GetByUnitAsync(long unitId, CancellationToken cancellationToken = default)
        => await _context.AuditMasters
            .Include(a => a.Observations)
            .Where(a => a.AuditUnit == unitId)
            .ToListAsync(cancellationToken);

    public async Task AddAsync(AuditMaster audit, CancellationToken cancellationToken = default)
        => await _context.AuditMasters.AddAsync(audit, cancellationToken);

    public Task UpdateAsync(AuditMaster audit, CancellationToken cancellationToken = default)
    {
        _context.AuditMasters.Update(audit);
        return Task.CompletedTask;
    }

    public async Task DeleteAsync(long auditId, CancellationToken cancellationToken = default)
    {
        var audit = await GetByIdAsync(auditId, cancellationToken);
        if (audit is not null) _context.AuditMasters.Remove(audit);
    }

    public async Task<bool> ExistsAsync(long auditId, CancellationToken cancellationToken = default)
        => await _context.AuditMasters.AnyAsync(a => a.AuditId == auditId, cancellationToken);
}

public class ObservationRepository : IObservationRepository
{
    private readonly AuditDbContext _context;

    public ObservationRepository(AuditDbContext context) => _context = context;

    public async Task<AuditObservation?> GetByIdAsync(long obvId, CancellationToken cancellationToken = default)
        => await _context.AuditObservations.FirstOrDefaultAsync(o => o.ObvId == obvId, cancellationToken);

    public async Task<IEnumerable<AuditObservation>> GetByAuditIdAsync(long auditId, CancellationToken cancellationToken = default)
        => await _context.AuditObservations.Where(o => o.ObvAuditId == auditId).ToListAsync(cancellationToken);

    public async Task<IEnumerable<AuditObservation>> GetPendingObservationsAsync(CancellationToken cancellationToken = default)
        => await _context.AuditObservations.Where(o => o.ObvStatus == 'P').ToListAsync(cancellationToken);

    public async Task AddAsync(AuditObservation observation, CancellationToken cancellationToken = default)
        => await _context.AuditObservations.AddAsync(observation, cancellationToken);

    public Task UpdateAsync(AuditObservation observation, CancellationToken cancellationToken = default)
    {
        _context.AuditObservations.Update(observation);
        return Task.CompletedTask;
    }

    public async Task<bool> ExistsAsync(long obvId, CancellationToken cancellationToken = default)
        => await _context.AuditObservations.AnyAsync(o => o.ObvId == obvId, cancellationToken);
}

public class GoodPracticeRepository : IGoodPracticeRepository
{
    private readonly AuditDbContext _context;

    public GoodPracticeRepository(AuditDbContext context) => _context = context;

    public async Task<AuditGoodPractice?> GetByIdAsync(long practiceId, CancellationToken cancellationToken = default)
        => await _context.AuditGoodPractices
            .Include(p => p.Ratings)
            .FirstOrDefaultAsync(p => p.PracticeId == practiceId, cancellationToken);

    public async Task<IEnumerable<AuditGoodPractice>> GetAllAsync(CancellationToken cancellationToken = default)
        => await _context.AuditGoodPractices.Include(p => p.Ratings).ToListAsync(cancellationToken);

    public async Task<IEnumerable<AuditGoodPractice>> GetByUnitAsync(long unitId, CancellationToken cancellationToken = default)
        => await _context.AuditGoodPractices
            .Include(p => p.Ratings)
            .Where(p => p.PracticeUnit == unitId)
            .ToListAsync(cancellationToken);

    public async Task AddAsync(AuditGoodPractice practice, CancellationToken cancellationToken = default)
        => await _context.AuditGoodPractices.AddAsync(practice, cancellationToken);

    public Task UpdateAsync(AuditGoodPractice practice, CancellationToken cancellationToken = default)
    {
        _context.AuditGoodPractices.Update(practice);
        return Task.CompletedTask;
    }
}

public class UnitOfWork : IUnitOfWork
{
    private readonly AuditDbContext _context;
    private readonly IPublisher _publisher;
    private IDbContextTransaction? _transaction;

    public UnitOfWork(AuditDbContext context, IPublisher publisher)
    {
        _context = context;
        _publisher = publisher;
    }

    public async Task<int> SaveChangesAsync(CancellationToken cancellationToken = default)
    {
        var domainEntities = _context.ChangeTracker
            .Entries<BaseEntity>()
            .Where(e => e.Entity.DomainEvents.Any())
            .ToList();

        var domainEvents = domainEntities
            .SelectMany(e => e.Entity.DomainEvents)
            .ToList();

        domainEntities.ForEach(e => e.Entity.ClearDomainEvents());

        var result = await _context.SaveChangesAsync(cancellationToken);

        foreach (var domainEvent in domainEvents)
            await _publisher.Publish(domainEvent, cancellationToken);

        return result;
    }

    public async Task BeginTransactionAsync(CancellationToken cancellationToken = default)
        => _transaction = await _context.Database.BeginTransactionAsync(cancellationToken);

    public async Task CommitTransactionAsync(CancellationToken cancellationToken = default)
    {
        if (_transaction is not null)
        {
            await _transaction.CommitAsync(cancellationToken);
            await _transaction.DisposeAsync();
            _transaction = null;
        }
    }

    public async Task RollbackTransactionAsync(CancellationToken cancellationToken = default)
    {
        if (_transaction is not null)
        {
            await _transaction.RollbackAsync(cancellationToken);
            await _transaction.DisposeAsync();
            _transaction = null;
        }
    }
}
