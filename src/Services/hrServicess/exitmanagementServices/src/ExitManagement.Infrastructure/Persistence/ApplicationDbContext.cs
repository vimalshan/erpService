using ExitManagement.Application.Common.Interfaces;
using ExitManagement.Domain.Entities;
using Microsoft.EntityFrameworkCore;

namespace ExitManagement.Infrastructure.Persistence;

public class ApplicationDbContext : DbContext, IApplicationDbContext
{
    public ApplicationDbContext(DbContextOptions<ApplicationDbContext> options) : base(options) { }

    public DbSet<EmployeeExit> EmployeeExits => Set<EmployeeExit>();
    public DbSet<ExitInterviewFeedback> ExitInterviewFeedbacks => Set<ExitInterviewFeedback>();
    public DbSet<ExitQuestion> ExitQuestions => Set<ExitQuestion>();
    public DbSet<ExitInterviewQuestion> ExitInterviewQuestions => Set<ExitInterviewQuestion>();
    public DbSet<ExitResponsibilityMap> ExitResponsibilityMaps => Set<ExitResponsibilityMap>();

    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        modelBuilder.ApplyConfigurationsFromAssembly(typeof(ApplicationDbContext).Assembly);
        base.OnModelCreating(modelBuilder);
    }

    public override async Task<int> SaveChangesAsync(CancellationToken cancellationToken = default)
    {
        return await base.SaveChangesAsync(cancellationToken);
    }
}
