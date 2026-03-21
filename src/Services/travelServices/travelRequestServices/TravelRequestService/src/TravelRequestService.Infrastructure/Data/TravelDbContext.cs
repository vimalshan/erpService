using Microsoft.EntityFrameworkCore;
using TravelRequestService.Domain.Common;
using TravelRequestService.Domain.Entities;

namespace TravelRequestService.Infrastructure.Data;

public class TravelDbContext : DbContext
{
    public TravelDbContext(DbContextOptions<TravelDbContext> options) : base(options) { }

    public DbSet<TravelMain> TravelMains => Set<TravelMain>();
    public DbSet<TravelSub> TravelSubs => Set<TravelSub>();
    public DbSet<TravelPersonal> TravelPersonals => Set<TravelPersonal>();
    public DbSet<TravelAgenda> TravelAgendas => Set<TravelAgenda>();
    public DbSet<TravelAdvance> TravelAdvances => Set<TravelAdvance>();
    public DbSet<TravelApprovalRemark> TravelApprovalRemarks => Set<TravelApprovalRemark>();
    public DbSet<DashTourPlan> DashTourPlans => Set<DashTourPlan>();

    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        modelBuilder.ApplyConfigurationsFromAssembly(typeof(TravelDbContext).Assembly);
        base.OnModelCreating(modelBuilder);
    }

    public override async Task<int> SaveChangesAsync(CancellationToken cancellationToken = default)
    {
        return await base.SaveChangesAsync(cancellationToken);
    }
}
