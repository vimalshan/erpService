using ProjectService.Domain.Entities;

namespace ProjectService.Domain.Interfaces;

public interface IProjectMainRepository : IRepository<ProjectMain>
{
    Task<ProjectMain?> GetProjectWithDetailsAsync(long projectId, CancellationToken cancellationToken = default);
    Task<IReadOnlyList<ProjectMain>> GetProjectsByStatusAsync(char status, CancellationToken cancellationToken = default);
    Task<IReadOnlyList<ProjectMain>> GetProjectsByLeaderAsync(long leaderId, CancellationToken cancellationToken = default);
    Task<IReadOnlyList<ProjectMain>> GetProjectsByTypeAsync(long typeId, CancellationToken cancellationToken = default);
}

public interface IProjectMasterRepository : IRepository<ProjectMaster>
{
    Task<IReadOnlyList<ProjectMaster>> GetByCategoryAsync(long categoryId, CancellationToken cancellationToken = default);
}

public interface IProjectTypeMasterRepository : IRepository<ProjectTypeMaster>
{
    Task<ProjectTypeMaster?> GetWithMappingsAsync(decimal typeId, CancellationToken cancellationToken = default);
}

public interface IProjectMemberRepository : IRepository<ProjectMember>
{
    Task<IReadOnlyList<ProjectMember>> GetByProjectAsync(long projectId, CancellationToken cancellationToken = default);
}
