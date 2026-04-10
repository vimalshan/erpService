using TaskTransactional.Domain.Entities;

namespace TaskTransactional.Domain.Interfaces;

public interface IComplaintMainRepository
{
    Task<ComplaintMain?> GetByGroupIdAsync(string groupId, CancellationToken ct = default);
    Task<IEnumerable<ComplaintMain>> GetAllAsync(CancellationToken ct = default);
    Task<IEnumerable<ComplaintMain>> GetByUnitCodeAsync(string unitCode, CancellationToken ct = default);
    Task AddAsync(ComplaintMain entity, CancellationToken ct = default);
    void Update(ComplaintMain entity);
    void Delete(ComplaintMain entity);
}

public interface IComplaintDetailRepository
{
    Task<ComplaintDetail?> GetByTicketNumAsync(decimal ticketNum, CancellationToken ct = default);
    Task<IEnumerable<ComplaintDetail>> GetAllAsync(CancellationToken ct = default);
    Task<IEnumerable<ComplaintDetail>> GetByGroupIdAsync(decimal groupId, CancellationToken ct = default);
    Task AddAsync(ComplaintDetail entity, CancellationToken ct = default);
    void Update(ComplaintDetail entity);
    void Delete(ComplaintDetail entity);
}

public interface IComplaintTaskRepository
{
    Task<ComplaintTask?> GetByTaskNumAsync(decimal taskNum, CancellationToken ct = default);
    Task<IEnumerable<ComplaintTask>> GetByTicketNumAsync(decimal ticketNum, CancellationToken ct = default);
    Task AddAsync(ComplaintTask entity, CancellationToken ct = default);
    void Update(ComplaintTask entity);
}

public interface IComplaintActionRepository
{
    Task<ComplaintAction?> GetByActionNumAsync(decimal actionNum, CancellationToken ct = default);
    Task<ComplaintAction?> GetByTaskNumAsync(decimal taskNum, CancellationToken ct = default);
    Task<IEnumerable<ComplaintAction>> GetAllAsync(CancellationToken ct = default);
    Task AddAsync(ComplaintAction entity, CancellationToken ct = default);
    void Update(ComplaintAction entity);
}

public interface IComplaintHistoryRepository
{
    Task<ComplaintHistory?> GetByHistoryNumAsync(decimal historyNum, CancellationToken ct = default);
    Task<IEnumerable<ComplaintHistory>> GetByActionNumAsync(decimal actionNum, CancellationToken ct = default);
    Task AddAsync(ComplaintHistory entity, CancellationToken ct = default);
}

public interface IComplaintEscalationRepository
{
    Task<IEnumerable<ComplaintEscalation>> GetByTicketNumAsync(decimal ticketNum, CancellationToken ct = default);
    Task AddAsync(ComplaintEscalation entity, CancellationToken ct = default);
    void Update(ComplaintEscalation entity);
}

public interface IUnitOfWork : IDisposable
{
    IComplaintMainRepository ComplaintMains { get; }
    IComplaintDetailRepository ComplaintDetails { get; }
    IComplaintTaskRepository ComplaintTasks { get; }
    IComplaintActionRepository ComplaintActions { get; }
    IComplaintHistoryRepository ComplaintHistories { get; }
    IComplaintEscalationRepository ComplaintEscalations { get; }
    Task<int> SaveChangesAsync(CancellationToken ct = default);
}
