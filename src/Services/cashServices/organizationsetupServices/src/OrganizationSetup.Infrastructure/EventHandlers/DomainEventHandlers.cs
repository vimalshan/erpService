using MediatR;
using Microsoft.Extensions.Logging;
using Microsoft.Extensions.Options;
using OrganizationSetup.Application.Interfaces;
using OrganizationSetup.Domain.Events;
using OrganizationSetup.Infrastructure.Messaging;

namespace OrganizationSetup.Infrastructure.EventHandlers;

public class RoleCreatedEventHandler : INotificationHandler<RoleCreatedEvent>
{
    private readonly IMessagePublisher _publisher;
    private readonly RabbitMqSettings _settings;
    private readonly ILogger<RoleCreatedEventHandler> _logger;

    public RoleCreatedEventHandler(IMessagePublisher publisher, IOptions<RabbitMqSettings> settings, ILogger<RoleCreatedEventHandler> logger)
    {
        _publisher = publisher;
        _settings = settings.Value;
        _logger = logger;
    }

    public async Task Handle(RoleCreatedEvent notification, CancellationToken cancellationToken)
    {
        _logger.LogInformation("Handling RoleCreatedEvent for RoleId={RoleId}", notification.RoleId);
        await _publisher.PublishAsync(_settings.ExchangeName, "role.created", notification, cancellationToken);
    }
}

public class RoleUpdatedEventHandler : INotificationHandler<RoleUpdatedEvent>
{
    private readonly IMessagePublisher _publisher;
    private readonly RabbitMqSettings _settings;
    private readonly ILogger<RoleUpdatedEventHandler> _logger;

    public RoleUpdatedEventHandler(IMessagePublisher publisher, IOptions<RabbitMqSettings> settings, ILogger<RoleUpdatedEventHandler> logger)
    {
        _publisher = publisher;
        _settings = settings.Value;
        _logger = logger;
    }

    public async Task Handle(RoleUpdatedEvent notification, CancellationToken cancellationToken)
    {
        _logger.LogInformation("Handling RoleUpdatedEvent for RoleId={RoleId}", notification.RoleId);
        await _publisher.PublishAsync(_settings.ExchangeName, "role.updated", notification, cancellationToken);
    }
}

public class RoleDeletedEventHandler : INotificationHandler<RoleDeletedEvent>
{
    private readonly IMessagePublisher _publisher;
    private readonly RabbitMqSettings _settings;
    private readonly ILogger<RoleDeletedEventHandler> _logger;

    public RoleDeletedEventHandler(IMessagePublisher publisher, IOptions<RabbitMqSettings> settings, ILogger<RoleDeletedEventHandler> logger)
    {
        _publisher = publisher;
        _settings = settings.Value;
        _logger = logger;
    }

    public async Task Handle(RoleDeletedEvent notification, CancellationToken cancellationToken)
    {
        _logger.LogInformation("Handling RoleDeletedEvent for RoleId={RoleId}", notification.RoleId);
        await _publisher.PublishAsync(_settings.ExchangeName, "role.deleted", notification, cancellationToken);
    }
}

public class OrgParamUpdatedEventHandler : INotificationHandler<OrgParamUpdatedEvent>
{
    private readonly IMessagePublisher _publisher;
    private readonly RabbitMqSettings _settings;
    private readonly ILogger<OrgParamUpdatedEventHandler> _logger;

    public OrgParamUpdatedEventHandler(IMessagePublisher publisher, IOptions<RabbitMqSettings> settings, ILogger<OrgParamUpdatedEventHandler> logger)
    {
        _publisher = publisher;
        _settings = settings.Value;
        _logger = logger;
    }

    public async Task Handle(OrgParamUpdatedEvent notification, CancellationToken cancellationToken)
    {
        _logger.LogInformation("Handling OrgParamUpdatedEvent for ParamId={ParamId}", notification.ParamId);
        await _publisher.PublishAsync(_settings.ExchangeName, "orgparam.updated", notification, cancellationToken);
    }
}

public class OrgParamDeletedEventHandler : INotificationHandler<OrgParamDeletedEvent>
{
    private readonly IMessagePublisher _publisher;
    private readonly RabbitMqSettings _settings;
    private readonly ILogger<OrgParamDeletedEventHandler> _logger;

    public OrgParamDeletedEventHandler(IMessagePublisher publisher, IOptions<RabbitMqSettings> settings, ILogger<OrgParamDeletedEventHandler> logger)
    {
        _publisher = publisher;
        _settings = settings.Value;
        _logger = logger;
    }

    public async Task Handle(OrgParamDeletedEvent notification, CancellationToken cancellationToken)
    {
        _logger.LogInformation("Handling OrgParamDeletedEvent for ParamId={ParamId}", notification.ParamId);
        await _publisher.PublishAsync(_settings.ExchangeName, "orgparam.deleted", notification, cancellationToken);
    }
}

public class UserMappedToRoleEventHandler : INotificationHandler<UserMappedToRoleEvent>
{
    private readonly IMessagePublisher _publisher;
    private readonly RabbitMqSettings _settings;
    private readonly ILogger<UserMappedToRoleEventHandler> _logger;

    public UserMappedToRoleEventHandler(IMessagePublisher publisher, IOptions<RabbitMqSettings> settings, ILogger<UserMappedToRoleEventHandler> logger)
    {
        _publisher = publisher;
        _settings = settings.Value;
        _logger = logger;
    }

    public async Task Handle(UserMappedToRoleEvent notification, CancellationToken cancellationToken)
    {
        _logger.LogInformation("Handling UserMappedToRoleEvent for MapId={MapId}", notification.MapId);
        await _publisher.PublishAsync(_settings.ExchangeName, "usermap.created", notification, cancellationToken);
    }
}

public class UserUnmappedFromRoleEventHandler : INotificationHandler<UserUnmappedFromRoleEvent>
{
    private readonly IMessagePublisher _publisher;
    private readonly RabbitMqSettings _settings;
    private readonly ILogger<UserUnmappedFromRoleEventHandler> _logger;

    public UserUnmappedFromRoleEventHandler(IMessagePublisher publisher, IOptions<RabbitMqSettings> settings, ILogger<UserUnmappedFromRoleEventHandler> logger)
    {
        _publisher = publisher;
        _settings = settings.Value;
        _logger = logger;
    }

    public async Task Handle(UserUnmappedFromRoleEvent notification, CancellationToken cancellationToken)
    {
        _logger.LogInformation("Handling UserUnmappedFromRoleEvent for MapId={MapId}", notification.MapId);
        await _publisher.PublishAsync(_settings.ExchangeName, "usermap.deleted", notification, cancellationToken);
    }
}

public class PpLimitSetEventHandler : INotificationHandler<PpLimitSetEvent>
{
    private readonly IMessagePublisher _publisher;
    private readonly RabbitMqSettings _settings;
    private readonly ILogger<PpLimitSetEventHandler> _logger;

    public PpLimitSetEventHandler(IMessagePublisher publisher, IOptions<RabbitMqSettings> settings, ILogger<PpLimitSetEventHandler> logger)
    {
        _publisher = publisher;
        _settings = settings.Value;
        _logger = logger;
    }

    public async Task Handle(PpLimitSetEvent notification, CancellationToken cancellationToken)
    {
        _logger.LogInformation("Handling PpLimitSetEvent for LimitId={LimitId}", notification.LimitId);
        await _publisher.PublishAsync(_settings.ExchangeName, "pplimit.created", notification, cancellationToken);
    }
}

public class PpLimitUpdatedEventHandler : INotificationHandler<PpLimitUpdatedEvent>
{
    private readonly IMessagePublisher _publisher;
    private readonly RabbitMqSettings _settings;
    private readonly ILogger<PpLimitUpdatedEventHandler> _logger;

    public PpLimitUpdatedEventHandler(IMessagePublisher publisher, IOptions<RabbitMqSettings> settings, ILogger<PpLimitUpdatedEventHandler> logger)
    {
        _publisher = publisher;
        _settings = settings.Value;
        _logger = logger;
    }

    public async Task Handle(PpLimitUpdatedEvent notification, CancellationToken cancellationToken)
    {
        _logger.LogInformation("Handling PpLimitUpdatedEvent for LimitId={LimitId}", notification.LimitId);
        await _publisher.PublishAsync(_settings.ExchangeName, "pplimit.updated", notification, cancellationToken);
    }
}

public class PpCertificateUploadedEventHandler : INotificationHandler<PpCertificateUploadedEvent>
{
    private readonly IMessagePublisher _publisher;
    private readonly RabbitMqSettings _settings;
    private readonly ILogger<PpCertificateUploadedEventHandler> _logger;

    public PpCertificateUploadedEventHandler(IMessagePublisher publisher, IOptions<RabbitMqSettings> settings, ILogger<PpCertificateUploadedEventHandler> logger)
    {
        _publisher = publisher;
        _settings = settings.Value;
        _logger = logger;
    }

    public async Task Handle(PpCertificateUploadedEvent notification, CancellationToken cancellationToken)
    {
        _logger.LogInformation("Handling PpCertificateUploadedEvent for LimitId={LimitId}", notification.LimitId);
        await _publisher.PublishAsync(_settings.ExchangeName, "pplimit.certificate.uploaded", notification, cancellationToken);
    }
}
