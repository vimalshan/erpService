using Microsoft.EntityFrameworkCore;
using ProjectService.Domain.Entities;
using ProjectService.Domain.Interfaces;
using ProjectService.Infrastructure.Data;

namespace ProjectService.Infrastructure.Repositories;

public class ProjectMainRepository(ProjectDbContext context)
    : Repository<ProjectMain>(context), IProjectMainRepository
{
    public async Task<ProjectMain?> GetProjectWithDetailsAsync(long projectId, CancellationToken cancellationToken = default)
    {
        return await Context.ProjectMains
            .Include(p => p.Members).ThenInclude(m => m.Function)
            .Include(p => p.Scopes)
            .Include(p => p.StatusHistory)
            .Include(p => p.AdditionalDeliverables)
            .Include(p => p.AdditionalScopes)
            .Include(p => p.ApprovalDetails)
            .Include(p => p.Deliverables)
            .Include(p => p.Holds)
            .Include(p => p.ProjectType)
            .Include(p => p.Location)
            .Include(p => p.Process)
            .AsSplitQuery()
            .FirstOrDefaultAsync(p => p.ProjId == projectId, cancellationToken);
    }

    public async Task<IReadOnlyList<ProjectMain>> GetProjectsByStatusAsync(char status, CancellationToken cancellationToken = default)
    {
        return await Context.ProjectMains
            .Where(p => p.ProjStatus == status)
            .Include(p => p.ProjectType)
            .Include(p => p.Location)
            .ToListAsync(cancellationToken);
    }

    public async Task<IReadOnlyList<ProjectMain>> GetProjectsByLeaderAsync(long leaderId, CancellationToken cancellationToken = default)
    {
        return await Context.ProjectMains
            .Where(p => p.ProjLeaderId == leaderId)
            .Include(p => p.ProjectType)
            .ToListAsync(cancellationToken);
    }

    public async Task<IReadOnlyList<ProjectMain>> GetProjectsByTypeAsync(long typeId, CancellationToken cancellationToken = default)
    {
        return await Context.ProjectMains
            .Where(p => p.ProjTypeId == typeId)
            .ToListAsync(cancellationToken);
    }
}

public class ProjectMasterRepository(ProjectDbContext context)
    : Repository<ProjectMaster>(context), IProjectMasterRepository
{
    public async Task<IReadOnlyList<ProjectMaster>> GetByCategoryAsync(long categoryId, CancellationToken cancellationToken = default)
    {
        return await Context.ProjectMasters
            .Where(p => p.ProjectCategoryId == categoryId)
            .Include(p => p.Category)
            .ToListAsync(cancellationToken);
    }
}

public class ProjectTypeMasterRepository(ProjectDbContext context)
    : Repository<ProjectTypeMaster>(context), IProjectTypeMasterRepository
{
    public async Task<ProjectTypeMaster?> GetWithMappingsAsync(decimal typeId, CancellationToken cancellationToken = default)
    {
        return await Context.ProjectTypeMasters
            .Include(t => t.DeliverableMaps)
            .Include(t => t.ObjectiveMaps)
            .Include(t => t.ScopeMaps)
            .Include(t => t.FunctionMaps).ThenInclude(f => f.Function)
            .Include(t => t.Category)
            .AsSplitQuery()
            .FirstOrDefaultAsync(t => t.ProjTypeId == typeId, cancellationToken);
    }
}

public class ProjectMemberRepository(ProjectDbContext context)
    : Repository<ProjectMember>(context), IProjectMemberRepository
{
    public async Task<IReadOnlyList<ProjectMember>> GetByProjectAsync(long projectId, CancellationToken cancellationToken = default)
    {
        return await Context.ProjectMembers
            .Where(m => m.ProjMemProjId == projectId)
            .Include(m => m.Function)
            .ToListAsync(cancellationToken);
    }
}
