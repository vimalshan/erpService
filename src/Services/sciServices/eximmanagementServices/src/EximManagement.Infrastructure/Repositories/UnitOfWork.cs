using EximManagement.Application.Interfaces;
using EximManagement.Infrastructure.Data;

namespace EximManagement.Infrastructure.Repositories;

public class UnitOfWork(EximDbContext db) : IUnitOfWork
{
    public Task<int> SaveChangesAsync(CancellationToken ct = default)
        => db.SaveChangesAsync(ct);
}
