using Microsoft.EntityFrameworkCore;
using EmployeeService.Domain.Entities;
using EmployeeService.Domain.Common;
using MediatR;

namespace EmployeeService.Infrastructure.Persistence;

public sealed class EmployeeDbContext : DbContext
{
    private readonly IMediator _mediator;

    public EmployeeDbContext(DbContextOptions<EmployeeDbContext> options, IMediator mediator)
        : base(options)
    {
        _mediator = mediator;
    }

    public DbSet<EmployeeTimeInfo> EmpTimeInfos => Set<EmployeeTimeInfo>();
    public DbSet<EmployeeApprover> EmployeeApprovers => Set<EmployeeApprover>();
    public DbSet<EmployeeApprovalMail> EmployeeApprovalMails => Set<EmployeeApprovalMail>();
    public DbSet<EmployeeCalendar> EmployeeCalendars => Set<EmployeeCalendar>();
    public DbSet<EmployeePattern> EmployeePatterns => Set<EmployeePattern>();
    public DbSet<EmployeeShift> EmployeeShifts => Set<EmployeeShift>();
    public DbSet<EmployeeShiftPattern> EmployeeShiftPatterns => Set<EmployeeShiftPattern>();

    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        modelBuilder.ApplyConfigurationsFromAssembly(typeof(EmployeeDbContext).Assembly);
        base.OnModelCreating(modelBuilder);
    }

    public override async Task<int> SaveChangesAsync(CancellationToken cancellationToken = default)
    {
        var result = await base.SaveChangesAsync(cancellationToken);
        await DispatchDomainEventsAsync(cancellationToken);
        return result;
    }

    private async Task DispatchDomainEventsAsync(CancellationToken ct)
    {
        var entities = ChangeTracker.Entries<BaseEntity>()
            .Where(e => e.Entity.DomainEvents.Any())
            .Select(e => e.Entity)
            .ToList();

        var events = entities.SelectMany(e => e.DomainEvents).ToList();
        entities.ForEach(e => e.ClearDomainEvents());

        foreach (var domainEvent in events)
            await _mediator.Publish((INotification)domainEvent, ct);
    }
}
