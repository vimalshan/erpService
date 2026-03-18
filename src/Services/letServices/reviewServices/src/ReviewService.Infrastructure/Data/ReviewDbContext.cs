using Microsoft.EntityFrameworkCore;
using ReviewService.Domain.Entities;
using ReviewService.Domain.Interfaces;
using MediatR;

namespace ReviewService.Infrastructure.Data;

public class ReviewDbContext : DbContext, IUnitOfWork
{
    private readonly IMediator _mediator;

    public ReviewDbContext(DbContextOptions<ReviewDbContext> options, IMediator mediator)
        : base(options)
    {
        _mediator = mediator;
    }

    public DbSet<ReviewMain> ReviewMains => Set<ReviewMain>();
    public DbSet<ReviewSub> ReviewSubs => Set<ReviewSub>();
    public DbSet<ReviewMast> ReviewMasts => Set<ReviewMast>();
    public DbSet<ReviewSkill> ReviewSkills => Set<ReviewSkill>();
    public DbSet<CourseFeedMain> CourseFeedMains => Set<CourseFeedMain>();
    public DbSet<CourseFeedSub> CourseFeedSubs => Set<CourseFeedSub>();
    public DbSet<CourseFeedbackMain> CourseFeedbackMains => Set<CourseFeedbackMain>();
    public DbSet<CourseFeedbackSub> CourseFeedbackSubs => Set<CourseFeedbackSub>();
    public DbSet<CourseReviewMain> CourseReviewMains => Set<CourseReviewMain>();
    public DbSet<CourseReviewSub> CourseReviewSubs => Set<CourseReviewSub>();
    public DbSet<FeedEvalMast> FeedEvalMasts => Set<FeedEvalMast>();
    public DbSet<FeedMast> FeedMasts => Set<FeedMast>();
    public DbSet<TrainerFeed> TrainerFeeds => Set<TrainerFeed>();

    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        modelBuilder.ApplyConfigurationsFromAssembly(typeof(ReviewDbContext).Assembly);
        base.OnModelCreating(modelBuilder);
    }

    public override async Task<int> SaveChangesAsync(CancellationToken cancellationToken = default)
    {
        await DispatchDomainEventsAsync(cancellationToken);
        return await base.SaveChangesAsync(cancellationToken);
    }

    private async Task DispatchDomainEventsAsync(CancellationToken cancellationToken)
    {
        var entities = ChangeTracker.Entries<Domain.Common.BaseEntity>()
            .Where(e => e.Entity.DomainEvents.Any())
            .Select(e => e.Entity)
            .ToList();

        var domainEvents = entities.SelectMany(e => e.DomainEvents).ToList();
        entities.ForEach(e => e.ClearDomainEvents());

        foreach (var domainEvent in domainEvents)
            await _mediator.Publish(domainEvent, cancellationToken);
    }
}
