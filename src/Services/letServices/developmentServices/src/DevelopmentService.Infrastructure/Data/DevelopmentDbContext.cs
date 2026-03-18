using Microsoft.EntityFrameworkCore;
using DevelopmentService.Domain.Entities;

namespace DevelopmentService.Infrastructure.Data;

public class DevelopmentDbContext : DbContext
{
    public DevelopmentDbContext(DbContextOptions<DevelopmentDbContext> options) : base(options) { }

    public DbSet<LetPlan> LetPlans { get; set; } = null!;
    public DbSet<LetPlanProb> LetPlanProbs { get; set; } = null!;
    public DbSet<LetBhrPlan> LetBhrPlans { get; set; } = null!;
    public DbSet<ReqNumCompeInd> ReqNumCompeInds { get; set; } = null!;
    public DbSet<CompetencyInd> CompetencyInds { get; set; } = null!;

    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        modelBuilder.ApplyConfigurationsFromAssembly(typeof(DevelopmentDbContext).Assembly);
        base.OnModelCreating(modelBuilder);
    }
}
