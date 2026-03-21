using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.Logging;

namespace SecurityService.Infrastructure.Messaging.Consumers;

public class UserCreatedConsumer : RabbitMqConsumerBase
{
    private readonly ILogger<UserCreatedConsumer> _logger;

    public UserCreatedConsumer(IConfiguration configuration, ILogger<UserCreatedConsumer> logger)
        : base(configuration, logger, "user-created")
    {
        _logger = logger;
    }

    protected override Task HandleMessageAsync(string message, CancellationToken ct)
    {
        _logger.LogInformation("Processing user-created event: {Message}", message);
        // Add business logic: send welcome email, audit log, etc.
        return Task.CompletedTask;
    }
}

public class UserDeactivatedConsumer : RabbitMqConsumerBase
{
    private readonly ILogger<UserDeactivatedConsumer> _logger;

    public UserDeactivatedConsumer(IConfiguration configuration, ILogger<UserDeactivatedConsumer> logger)
        : base(configuration, logger, "user-deactivated")
    {
        _logger = logger;
    }

    protected override Task HandleMessageAsync(string message, CancellationToken ct)
    {
        _logger.LogInformation("Processing user-deactivated event: {Message}", message);
        // Add business logic: revoke sessions, notify admins, etc.
        return Task.CompletedTask;
    }
}
