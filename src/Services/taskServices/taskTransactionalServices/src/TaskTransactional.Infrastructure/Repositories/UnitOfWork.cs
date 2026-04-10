using TaskTransactional.Domain.Interfaces;
using TaskTransactional.Infrastructure.Persistence;

namespace TaskTransactional.Infrastructure.Repositories;

public class UnitOfWork(ComplaintDbContext db) : IUnitOfWork
{
    private IComplaintMainRepository? _complaintMains;
    private IComplaintDetailRepository? _complaintDetails;
    private IComplaintTaskRepository? _complaintTasks;
    private IComplaintActionRepository? _complaintActions;
    private IComplaintHistoryRepository? _complaintHistories;
    private IComplaintEscalationRepository? _complaintEscalations;

    public IComplaintMainRepository ComplaintMains => _complaintMains ??= new ComplaintMainRepository(db);
    public IComplaintDetailRepository ComplaintDetails => _complaintDetails ??= new ComplaintDetailRepository(db);
    public IComplaintTaskRepository ComplaintTasks => _complaintTasks ??= new ComplaintTaskRepository(db);
    public IComplaintActionRepository ComplaintActions => _complaintActions ??= new ComplaintActionRepository(db);
    public IComplaintHistoryRepository ComplaintHistories => _complaintHistories ??= new ComplaintHistoryRepository(db);
    public IComplaintEscalationRepository ComplaintEscalations => _complaintEscalations ??= new ComplaintEscalationRepository(db);

    public async Task<int> SaveChangesAsync(CancellationToken ct = default) => await db.SaveChangesAsync(ct);
    public void Dispose() => db.Dispose();
}
