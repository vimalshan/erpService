using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using Microsoft.Extensions.Logging;
using MasterData.Domain.Events;
using MasterData.Application.Services;

#nullable enable

namespace MasterData.Application.EventHandlers
{
    /// <summary>
    /// Domain event handler for company unit events
    /// </summary>
    public interface IDomainEventHandler
    {
        Task HandleAsync(DomainEvent @event);
    }

    /// <summary>
    /// Handler for CompanyUnitCreatedEvent
    /// </summary>
    public class CompanyUnitCreatedEventHandler : IDomainEventHandler
    {
        private readonly IMessagePublisher _messagePublisher;
        private readonly ILogger<CompanyUnitCreatedEventHandler> _logger;

        public CompanyUnitCreatedEventHandler(IMessagePublisher messagePublisher, ILogger<CompanyUnitCreatedEventHandler> logger)
        {
            _messagePublisher = messagePublisher;
            _logger = logger;
        }

        public async Task HandleAsync(DomainEvent @event)
        {
            if (@event is not CompanyUnitCreatedEvent companyUnitEvent)
                return;

            _logger.LogInformation($"Company Unit created: {companyUnitEvent.Code}");
            await _messagePublisher.PublishCompanyUnitEventAsync("created", companyUnitEvent);
        }
    }

    /// <summary>
    /// Handler for CompanyUnitUpdatedEvent
    /// </summary>
    public class CompanyUnitUpdatedEventHandler : IDomainEventHandler
    {
        private readonly IMessagePublisher _messagePublisher;
        private readonly ILogger<CompanyUnitUpdatedEventHandler> _logger;

        public CompanyUnitUpdatedEventHandler(IMessagePublisher messagePublisher, ILogger<CompanyUnitUpdatedEventHandler> logger)
        {
            _messagePublisher = messagePublisher;
            _logger = logger;
        }

        public async Task HandleAsync(DomainEvent @event)
        {
            if (@event is not CompanyUnitUpdatedEvent companyUnitEvent)
                return;

            _logger.LogInformation($"Company Unit updated: {companyUnitEvent.Code}");
            await _messagePublisher.PublishCompanyUnitEventAsync("updated", companyUnitEvent);
        }
    }

    /// <summary>
    /// Handler for CompanyUnitDeletedEvent
    /// </summary>
    public class CompanyUnitDeletedEventHandler : IDomainEventHandler
    {
        private readonly IMessagePublisher _messagePublisher;
        private readonly ILogger<CompanyUnitDeletedEventHandler> _logger;

        public CompanyUnitDeletedEventHandler(IMessagePublisher messagePublisher, ILogger<CompanyUnitDeletedEventHandler> logger)
        {
            _messagePublisher = messagePublisher;
            _logger = logger;
        }

        public async Task HandleAsync(DomainEvent @event)
        {
            if (@event is not CompanyUnitDeletedEvent companyUnitEvent)
                return;

            _logger.LogInformation($"Company Unit deleted: {companyUnitEvent.Id}");
            await _messagePublisher.PublishCompanyUnitEventAsync("deleted", companyUnitEvent);
        }
    }

    /// <summary>
    /// Handler for SupplierCreatedEvent
    /// </summary>
    public class SupplierCreatedEventHandler : IDomainEventHandler
    {
        private readonly IMessagePublisher _messagePublisher;
        private readonly ILogger<SupplierCreatedEventHandler> _logger;

        public SupplierCreatedEventHandler(IMessagePublisher messagePublisher, ILogger<SupplierCreatedEventHandler> logger)
        {
            _messagePublisher = messagePublisher;
            _logger = logger;
        }

        public async Task HandleAsync(DomainEvent @event)
        {
            if (@event is not SupplierCreatedEvent supplierEvent)
                return;

            _logger.LogInformation($"Supplier created: {supplierEvent.Code}");
            await _messagePublisher.PublishSupplierEventAsync("created", supplierEvent);
        }
    }

    /// <summary>
    /// Handler for SupplierUpdatedEvent
    /// </summary>
    public class SupplierUpdatedEventHandler : IDomainEventHandler
    {
        private readonly IMessagePublisher _messagePublisher;
        private readonly ILogger<SupplierUpdatedEventHandler> _logger;

        public SupplierUpdatedEventHandler(IMessagePublisher messagePublisher, ILogger<SupplierUpdatedEventHandler> logger)
        {
            _messagePublisher = messagePublisher;
            _logger = logger;
        }

        public async Task HandleAsync(DomainEvent @event)
        {
            if (@event is not SupplierUpdatedEvent supplierEvent)
                return;

            _logger.LogInformation($"Supplier updated: {supplierEvent.Code}");
            await _messagePublisher.PublishSupplierEventAsync("updated", supplierEvent);
        }
    }

    /// <summary>
    /// Handler for SupplierDeletedEvent
    /// </summary>
    public class SupplierDeletedEventHandler : IDomainEventHandler
    {
        private readonly IMessagePublisher _messagePublisher;
        private readonly ILogger<SupplierDeletedEventHandler> _logger;

        public SupplierDeletedEventHandler(IMessagePublisher messagePublisher, ILogger<SupplierDeletedEventHandler> logger)
        {
            _messagePublisher = messagePublisher;
            _logger = logger;
        }

        public async Task HandleAsync(DomainEvent @event)
        {
            if (@event is not SupplierDeletedEvent supplierEvent)
                return;

            _logger.LogInformation($"Supplier deleted: {supplierEvent.Code}");
            await _messagePublisher.PublishSupplierEventAsync("deleted", supplierEvent);
        }
    }
}
