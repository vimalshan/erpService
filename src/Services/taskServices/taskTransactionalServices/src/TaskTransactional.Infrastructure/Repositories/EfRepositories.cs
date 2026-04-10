using TaskTransactional.Domain.Entities;
using TaskTransactional.Domain.Interfaces;
using TaskTransactional.Infrastructure.Persistence;
using Microsoft.EntityFrameworkCore;

namespace TaskTransactional.Infrastructure.Repositories;

public class ComplaintMainRepository(ComplaintDbContext db) : IComplaintMainRepository
{
    public async Task<ComplaintMain?> GetByGroupIdAsync(string groupId, CancellationToken ct = default)
        => await db.ComplaintMains.FirstOrDefaultAsync(x => x.CmGroupId == groupId, ct);

    public async Task<IEnumerable<ComplaintMain>> GetAllAsync(CancellationToken ct = default)
        => await db.ComplaintMains.ToListAsync(ct);

    public async Task<IEnumerable<ComplaintMain>> GetByUnitCodeAsync(string unitCode, CancellationToken ct = default)
        => await db.ComplaintMains.Where(x => x.CmUnitCode == unitCode).ToListAsync(ct);

    public async Task AddAsync(ComplaintMain entity, CancellationToken ct = default)
        => await db.ComplaintMains.AddAsync(entity, ct);

    public void Update(ComplaintMain entity) => db.ComplaintMains.Update(entity);
    public void Delete(ComplaintMain entity) => db.ComplaintMains.Remove(entity);
}

public class ComplaintDetailRepository(ComplaintDbContext db) : IComplaintDetailRepository
{
    public async Task<ComplaintDetail?> GetByTicketNumAsync(decimal ticketNum, CancellationToken ct = default)
        => await db.ComplaintDetails.FirstOrDefaultAsync(x => x.CdTicketNum == ticketNum, ct);

    public async Task<IEnumerable<ComplaintDetail>> GetAllAsync(CancellationToken ct = default)
        => await db.ComplaintDetails.ToListAsync(ct);

    public async Task<IEnumerable<ComplaintDetail>> GetByGroupIdAsync(decimal groupId, CancellationToken ct = default)
        => await db.ComplaintDetails.Where(x => x.CdGroupId == groupId).ToListAsync(ct);

    public async Task AddAsync(ComplaintDetail entity, CancellationToken ct = default)
        => await db.ComplaintDetails.AddAsync(entity, ct);

    public void Update(ComplaintDetail entity) => db.ComplaintDetails.Update(entity);
    public void Delete(ComplaintDetail entity) => db.ComplaintDetails.Remove(entity);
}

public class ComplaintTaskRepository(ComplaintDbContext db) : IComplaintTaskRepository
{
    public async Task<ComplaintTask?> GetByTaskNumAsync(decimal taskNum, CancellationToken ct = default)
        => await db.ComplaintTasks.FirstOrDefaultAsync(x => x.CtTaskNum == taskNum, ct);

    public async Task<IEnumerable<ComplaintTask>> GetByTicketNumAsync(decimal ticketNum, CancellationToken ct = default)
        => await db.ComplaintTasks.Where(x => x.CtTicketNum == ticketNum).ToListAsync(ct);

    public async Task AddAsync(ComplaintTask entity, CancellationToken ct = default)
        => await db.ComplaintTasks.AddAsync(entity, ct);

    public void Update(ComplaintTask entity) => db.ComplaintTasks.Update(entity);
}

public class ComplaintActionRepository(ComplaintDbContext db) : IComplaintActionRepository
{
    public async Task<ComplaintAction?> GetByActionNumAsync(decimal actionNum, CancellationToken ct = default)
        => await db.ComplaintActions.FirstOrDefaultAsync(x => x.CaActionNum == actionNum, ct);

    public async Task<ComplaintAction?> GetByTaskNumAsync(decimal taskNum, CancellationToken ct = default)
        => await db.ComplaintActions.FirstOrDefaultAsync(x => x.CaTaskNum == taskNum, ct);

    public async Task<IEnumerable<ComplaintAction>> GetAllAsync(CancellationToken ct = default)
        => await db.ComplaintActions.ToListAsync(ct);

    public async Task AddAsync(ComplaintAction entity, CancellationToken ct = default)
        => await db.ComplaintActions.AddAsync(entity, ct);

    public void Update(ComplaintAction entity) => db.ComplaintActions.Update(entity);
}

public class ComplaintHistoryRepository(ComplaintDbContext db) : IComplaintHistoryRepository
{
    public async Task<ComplaintHistory?> GetByHistoryNumAsync(decimal historyNum, CancellationToken ct = default)
        => await db.ComplaintHistories.FirstOrDefaultAsync(x => x.ChHistoryNum == historyNum, ct);

    public async Task<IEnumerable<ComplaintHistory>> GetByActionNumAsync(decimal actionNum, CancellationToken ct = default)
        => await db.ComplaintHistories.Where(x => x.ChActionNum == actionNum).ToListAsync(ct);

    public async Task AddAsync(ComplaintHistory entity, CancellationToken ct = default)
        => await db.ComplaintHistories.AddAsync(entity, ct);
}

public class ComplaintEscalationRepository(ComplaintDbContext db) : IComplaintEscalationRepository
{
    public async Task<IEnumerable<ComplaintEscalation>> GetByTicketNumAsync(decimal ticketNum, CancellationToken ct = default)
        => await db.ComplaintEscalations.Where(x => x.CeTicketNum == ticketNum).ToListAsync(ct);

    public async Task AddAsync(ComplaintEscalation entity, CancellationToken ct = default)
        => await db.ComplaintEscalations.AddAsync(entity, ct);

    public void Update(ComplaintEscalation entity) => db.ComplaintEscalations.Update(entity);
}
