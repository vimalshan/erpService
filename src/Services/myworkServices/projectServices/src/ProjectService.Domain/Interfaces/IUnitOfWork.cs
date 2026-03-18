namespace ProjectService.Domain.Interfaces;

public interface IUnitOfWork : IDisposable
{
    IProjectMainRepository ProjectMains { get; }
    IProjectMasterRepository ProjectMasters { get; }
    IProjectTypeMasterRepository ProjectTypes { get; }
    IProjectMemberRepository ProjectMembers { get; }
    Task<int> SaveChangesAsync(CancellationToken cancellationToken = default);
}
