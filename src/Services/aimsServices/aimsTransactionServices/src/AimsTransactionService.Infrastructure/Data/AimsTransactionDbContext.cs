using Microsoft.EntityFrameworkCore;
using AimsTransactionService.Domain.Aggregates;
using AimsTransactionService.Domain.Common;
using AimsTransactionService.Domain.Entities;

namespace AimsTransactionService.Infrastructure.Data;

public class AimsTransactionDbContext(DbContextOptions<AimsTransactionDbContext> options) : DbContext(options)
{
    public DbSet<SwipeAggregate> Swipes => Set<SwipeAggregate>();
    public DbSet<AttendanceBatchAggregate> AttendanceBatches => Set<AttendanceBatchAggregate>();
    public DbSet<LeaveApplicationAggregate> LeaveApplications => Set<LeaveApplicationAggregate>();
    public DbSet<CompOffAggregate> CompOffs => Set<CompOffAggregate>();
    public DbSet<AttendanceLopMain> AttendanceLopMains => Set<AttendanceLopMain>();
    public DbSet<AttendanceLopDetail> AttendanceLopDetails => Set<AttendanceLopDetail>();
    public DbSet<AttendanceOvertime> AttendanceOvertimes => Set<AttendanceOvertime>();
    public DbSet<AttendanceSummary> AttendanceSummaries => Set<AttendanceSummary>();
    public DbSet<LeaveApproval> LeaveApprovals => Set<LeaveApproval>();
    public DbSet<LeaveCredit> LeaveCredits => Set<LeaveCredit>();

    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        modelBuilder.ApplyConfigurationsFromAssembly(typeof(AimsTransactionDbContext).Assembly);
        base.OnModelCreating(modelBuilder);
    }

    public override async Task<int> SaveChangesAsync(CancellationToken cancellationToken = default)
    {
        var aggregates = ChangeTracker
            .Entries<Entity>()
            .Where(e => e.Entity.DomainEvents.Count != 0)
            .Select(e => e.Entity)
            .ToList();

        var domainEvents = aggregates
            .SelectMany(a => a.DomainEvents)
            .ToList();

        aggregates.ForEach(a => a.ClearDomainEvents());

        var result = await base.SaveChangesAsync(cancellationToken);

        if (domainEvents.Count != 0)
            DomainEventsBeforeSave = domainEvents;

        return result;
    }

    public List<IDomainEvent> DomainEventsBeforeSave { get; private set; } = [];
}
