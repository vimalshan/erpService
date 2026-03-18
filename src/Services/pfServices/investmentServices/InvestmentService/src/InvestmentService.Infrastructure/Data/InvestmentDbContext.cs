using InvestmentService.Domain.Common;
using InvestmentService.Domain.Entities;
using MediatR;
using Microsoft.EntityFrameworkCore;

namespace InvestmentService.Infrastructure.Data;

public class InvestmentDbContext : DbContext
{
    private readonly IMediator? _mediator;

    public InvestmentDbContext(DbContextOptions<InvestmentDbContext> options, IMediator? mediator = null)
        : base(options)
    {
        _mediator = mediator;
    }

    public DbSet<Investment> Investments => Set<Investment>();
    public DbSet<SaleDetail> SaleDetails => Set<SaleDetail>();
    public DbSet<ScheduleDetail> ScheduleDetails => Set<ScheduleDetail>();
    public DbSet<CallDetail> CallDetails => Set<CallDetail>();
    public DbSet<ApprovalDetail> ApprovalDetails => Set<ApprovalDetail>();
    public DbSet<InvestmentCategory> Categories => Set<InvestmentCategory>();
    public DbSet<InvestmentSubCategory> SubCategories => Set<InvestmentSubCategory>();
    public DbSet<CreditAgency> CreditAgencies => Set<CreditAgency>();
    public DbSet<CreditRating> CreditRatings => Set<CreditRating>();
    public DbSet<Broker> Brokers => Set<Broker>();
    public DbSet<InterestScheduleBatch> InterestScheduleBatches => Set<InterestScheduleBatch>();
    public DbSet<BankDetail> BankDetails => Set<BankDetail>();

    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        modelBuilder.ApplyConfigurationsFromAssembly(typeof(InvestmentDbContext).Assembly);
        base.OnModelCreating(modelBuilder);
    }

    public override async Task<int> SaveChangesAsync(CancellationToken ct = default)
    {
        var result = await base.SaveChangesAsync(ct);

        if (_mediator != null)
        {
            var entities = ChangeTracker.Entries<BaseEntity>()
                .Where(e => e.Entity.DomainEvents.Any())
                .Select(e => e.Entity)
                .ToList();

            var domainEvents = entities.SelectMany(e => e.DomainEvents).ToList();
            entities.ForEach(e => e.ClearDomainEvents());

            foreach (var domainEvent in domainEvents)
                await _mediator.Publish(domainEvent, ct);
        }

        return result;
    }
}
