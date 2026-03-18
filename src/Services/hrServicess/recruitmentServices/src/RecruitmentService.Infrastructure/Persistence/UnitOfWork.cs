using RecruitmentService.Application.Interfaces;
using RecruitmentService.Infrastructure.Persistence;

namespace RecruitmentService.Infrastructure.Persistence;

public class UnitOfWork : IUnitOfWork
{
    private readonly RecruitmentDbContext _context;

    public UnitOfWork(RecruitmentDbContext context) => _context = context;

    public Task<int> SaveChangesAsync(CancellationToken cancellationToken = default)
        => _context.SaveChangesAsync(cancellationToken);
}
