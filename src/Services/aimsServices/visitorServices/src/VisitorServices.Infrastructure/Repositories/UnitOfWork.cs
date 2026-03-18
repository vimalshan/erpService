using VisitorServices.Application.Common.Interfaces;
using VisitorServices.Infrastructure.Data;

namespace VisitorServices.Infrastructure.Repositories;

public class UnitOfWork(VisitorDbContext context) : IUnitOfWork
{
    public Task<int> SaveChangesAsync(CancellationToken cancellationToken = default)
        => context.SaveChangesAsync(cancellationToken);
}
