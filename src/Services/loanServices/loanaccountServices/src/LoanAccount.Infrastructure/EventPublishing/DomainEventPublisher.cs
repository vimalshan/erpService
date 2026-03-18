using Ardalis.GuardClauses;
using LoanAccount.Domain.Common;
using LoanAccount.Infrastructure.Messaging;
using LoanAccount.Infrastructure.Persistence;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Diagnostics;
using Microsoft.Extensions.Logging;

namespace LoanAccount.Infrastructure.EventPublishing;

/// <summary>
/// Service for publishing domain events to message queue
/// </summary>
public interface IDomainEventPublisher
{
    Task PublishEventsAsync(CancellationToken cancellationToken = default);
}

/// <summary>
/// Domain event publisher implementation
/// </summary>
public class DomainEventPublisher : IDomainEventPublisher
{
    private readonly LoanAccountDbContext _dbContext;
    private readonly IEventPublisher _eventPublisher;
    private readonly ILogger<DomainEventPublisher> _logger;

    public DomainEventPublisher(
        LoanAccountDbContext dbContext,
        IEventPublisher eventPublisher,
        ILogger<DomainEventPublisher> logger)
    {
        _dbContext = Guard.Against.Null(dbContext, nameof(dbContext));
        _eventPublisher = Guard.Against.Null(eventPublisher, nameof(eventPublisher));
        _logger = Guard.Against.Null(logger, nameof(logger));
    }

    /// <summary>
    /// Publishes all domain events from entities in the current context
    /// </summary>
    public async Task PublishEventsAsync(CancellationToken cancellationToken = default)
    {
        var entries = _dbContext.ChangeTracker.Entries<Entity>()
            .Where(e => e.Entity.GetDomainEvents().Any())
            .ToList();

        if (!entries.Any())
            return;

        try
        {
            foreach (var entry in entries)
            {
                var entity = entry.Entity;
                var events = entity.GetDomainEvents().ToList();

                foreach (var @event in events)
                {
                    var eventType = @event.GetType().Name;

                    _logger.LogInformation(
                        "Publishing domain event: {EventType} for aggregate {AggregateId}",
                        eventType, @event.AggregateId);

                    await _eventPublisher.PublishEventAsync(@event, eventType, cancellationToken);
                }

                entity.ClearDomainEvents();
            }

            _logger.LogInformation("Published {Count} domain events", entries.Sum(e => e.Entity.GetDomainEvents().Count));
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "Error publishing domain events");
            throw;
        }
    }
}

/// <summary>
/// Middleware to intercept SaveChanges and publish domain events
/// </summary>
public class DomainEventPublishingInterceptor : SaveChangesInterceptor
{
    private readonly IDomainEventPublisher _eventPublisher;
    private readonly ILogger<DomainEventPublishingInterceptor> _logger;

    public DomainEventPublishingInterceptor(
        IDomainEventPublisher eventPublisher,
        ILogger<DomainEventPublishingInterceptor> logger)
    {
        _eventPublisher = Guard.Against.Null(eventPublisher, nameof(eventPublisher));
        _logger = Guard.Against.Null(logger, nameof(logger));
    }

    public override async ValueTask<InterceptionResult<int>> SavingChangesAsync(
        DbContextEventData eventData,
        InterceptionResult<int> result,
        CancellationToken cancellationToken = default)
    {
        // Publish domain events before saving changes
        await _eventPublisher.PublishEventsAsync(cancellationToken);
        return await base.SavingChangesAsync(eventData, result, cancellationToken);
    }

    public override InterceptionResult<int> SavingChanges(
        DbContextEventData eventData,
        InterceptionResult<int> result)
    {
        // For synchronous save, we'll publish async
        _eventPublisher.PublishEventsAsync(CancellationToken.None).GetAwaiter().GetResult();
        return base.SavingChanges(eventData, result);
    }
}
