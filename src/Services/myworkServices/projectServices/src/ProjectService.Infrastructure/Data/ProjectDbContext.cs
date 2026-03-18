using Microsoft.EntityFrameworkCore;
using ProjectService.Domain.Common;
using ProjectService.Domain.Entities;

namespace ProjectService.Infrastructure.Data;

public class ProjectDbContext(DbContextOptions<ProjectDbContext> options) : DbContext(options)
{
    public DbSet<ProjectMain> ProjectMains => Set<ProjectMain>();
    public DbSet<ProjectMaster> ProjectMasters => Set<ProjectMaster>();
    public DbSet<ProjectMember> ProjectMembers => Set<ProjectMember>();
    public DbSet<ProjectScope> ProjectScopes => Set<ProjectScope>();
    public DbSet<ProjectStatusHistory> ProjectStatusHistories => Set<ProjectStatusHistory>();
    public DbSet<ProjectAdditionalDeliverable> ProjectAdditionalDeliverables => Set<ProjectAdditionalDeliverable>();
    public DbSet<ProjectAdditionalScope> ProjectAdditionalScopes => Set<ProjectAdditionalScope>();
    public DbSet<ProjectApprovalDetail> ProjectApprovalDetails => Set<ProjectApprovalDetail>();
    public DbSet<ProjectDeliverable> ProjectDeliverables => Set<ProjectDeliverable>();
    public DbSet<ProjectHold> ProjectHolds => Set<ProjectHold>();
    public DbSet<ProjectEmployeeMap> ProjectEmployeeMaps => Set<ProjectEmployeeMap>();
    public DbSet<ProjectAccess> ProjectAccesses => Set<ProjectAccess>();

    public DbSet<ProjectTypeMaster> ProjectTypeMasters => Set<ProjectTypeMaster>();
    public DbSet<ProjectCategoryMaster> ProjectCategoryMasters => Set<ProjectCategoryMaster>();
    public DbSet<ProjectTypeCategoryMaster> ProjectTypeCategoryMasters => Set<ProjectTypeCategoryMaster>();
    public DbSet<ProjectTypeDeliverableMap> ProjectTypeDeliverableMaps => Set<ProjectTypeDeliverableMap>();
    public DbSet<ProjectTypeObjectiveMap> ProjectTypeObjectiveMaps => Set<ProjectTypeObjectiveMap>();
    public DbSet<ProjectTypeScopeMap> ProjectTypeScopeMaps => Set<ProjectTypeScopeMap>();
    public DbSet<ProjectTypeFinYearSeq> ProjectTypeFinYearSeqs => Set<ProjectTypeFinYearSeq>();

    public DbSet<ProjectDepartment> ProjectDepartments => Set<ProjectDepartment>();
    public DbSet<ProjectLocation> ProjectLocations => Set<ProjectLocation>();
    public DbSet<ProjectProcess> ProjectProcesses => Set<ProjectProcess>();
    public DbSet<ProjectFunction> ProjectFunctions => Set<ProjectFunction>();
    public DbSet<ProjectFunctionEmployeeMap> ProjectFunctionEmployeeMaps => Set<ProjectFunctionEmployeeMap>();
    public DbSet<ProjectTypeFunctionMap> ProjectTypeFunctionMaps => Set<ProjectTypeFunctionMap>();

    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        modelBuilder.ApplyConfigurationsFromAssembly(typeof(ProjectDbContext).Assembly);
        base.OnModelCreating(modelBuilder);
    }

    public override async Task<int> SaveChangesAsync(CancellationToken cancellationToken = default)
    {
        return await base.SaveChangesAsync(cancellationToken);
    }
}
