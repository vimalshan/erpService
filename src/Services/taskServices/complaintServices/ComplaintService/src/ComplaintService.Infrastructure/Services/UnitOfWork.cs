using ComplaintService.Application.Interfaces;
using ComplaintService.Infrastructure.Persistence;

namespace ComplaintService.Infrastructure.Services;

public class UnitOfWork(ComplaintDbContext dbContext) : IUnitOfWork
{
    public Task<int> SaveChangesAsync(CancellationToken ct = default) =>
        dbContext.SaveChangesAsync(ct);
}
