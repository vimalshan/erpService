using MediatR;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Diagnostics;
using Microsoft.Extensions.Logging;
using SalesOrderService.Application.SalesOrders.EventHandlers;
using SalesOrderService.Domain.Common;
using SalesOrderService.Domain.Events;

namespace SalesOrderService.Infrastructure.DomainEvents;

public sealed class DomainEventDispatcherInterceptor(
    IMediator mediator,
    ILogger<DomainEventDispatcherInterceptor> logger) : SaveChangesInterceptor
{
    public override async ValueTask<int> SavedChangesAsync(
        SaveChangesCompletedEventData eventData,
        int result,
        CancellationToken cancellationToken = default)
    {
        try
        {
            await DispatchDomainEventsAsync(eventData.Context!, cancellationToken);
        }
        catch (Exception ex)
        {
            logger.LogError(ex, "Domain event dispatch failed — data was already saved.");
        }

        return await base.SavedChangesAsync(eventData, result, cancellationToken);
    }

    private async Task DispatchDomainEventsAsync(DbContext context, CancellationToken ct)
    {
        var entities = context.ChangeTracker.Entries<BaseEntity>()
            .Where(e => e.Entity.DomainEvents.Any())
            .Select(e => e.Entity)
            .ToList();

        var events = entities.SelectMany(e => e.DomainEvents).ToList();
        entities.ForEach(e => e.ClearDomainEvents());

        foreach (var domainEvent in events)
        {
            INotification notification = domainEvent switch
            {
                SalesOrderCreatedEvent e   => new SalesOrderCreatedDomainNotification(e),
                SalesOrderConfirmedEvent e => new SalesOrderConfirmedDomainNotification(e),
                SalesOrderCompletedEvent e => new SalesOrderCompletedDomainNotification(e),
                SalesOrderCancelledEvent e => new SalesOrderCancelledDomainNotification(e),
                _ => throw new InvalidOperationException($"Unhandled domain event: {domainEvent.GetType().Name}")
            };

            await mediator.Publish(notification, ct);
        }
    }
}
