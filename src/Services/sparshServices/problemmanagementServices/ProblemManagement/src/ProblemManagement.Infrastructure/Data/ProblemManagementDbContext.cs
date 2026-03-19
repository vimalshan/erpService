using Microsoft.EntityFrameworkCore;
using ProblemManagement.Domain.Common;
using ProblemManagement.Domain.Entities;
using ProblemManagement.Domain.Interfaces;
using MediatR;

namespace ProblemManagement.Infrastructure.Data;

public class ProblemManagementDbContext : DbContext, IUnitOfWork
{
    private readonly IMediator _mediator;

    public ProblemManagementDbContext(DbContextOptions<ProblemManagementDbContext> options, IMediator mediator)
        : base(options)
    {
        _mediator = mediator;
    }

    public DbSet<ProblemFunction> ProblemFunctions => Set<ProblemFunction>();
    public DbSet<ProblemImpact> ProblemImpacts => Set<ProblemImpact>();
    public DbSet<ProblemMain> Problems => Set<ProblemMain>();
    public DbSet<ProblemAttachment> ProblemAttachments => Set<ProblemAttachment>();
    public DbSet<ProblemSolution> ProblemSolutions => Set<ProblemSolution>();
    public DbSet<ProblemApproval> ProblemApprovals => Set<ProblemApproval>();
    public DbSet<ProblemAppAudience> ProblemAppAudiences => Set<ProblemAppAudience>();
    public DbSet<SolutionApproval> SolutionApprovals => Set<SolutionApproval>();
    public DbSet<SolutionComment> SolutionComments => Set<SolutionComment>();

    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        modelBuilder.ApplyConfigurationsFromAssembly(typeof(ProblemManagementDbContext).Assembly);
        base.OnModelCreating(modelBuilder);
    }

    public override async Task<int> SaveChangesAsync(CancellationToken ct = default)
    {
        var domainEvents = ChangeTracker.Entries<BaseEntity>()
            .SelectMany(e => e.Entity.DomainEvents)
            .ToList();

        var result = await base.SaveChangesAsync(ct);

        foreach (var domainEvent in domainEvents)
        {
            await _mediator.Publish(domainEvent, ct);
        }

        foreach (var entry in ChangeTracker.Entries<BaseEntity>())
        {
            entry.Entity.ClearDomainEvents();
        }

        return result;
    }
}
