using SettingsService.Domain.Events;
using MassTransit;
using Microsoft.Extensions.Logging;

namespace SettingsService.Infrastructure.Messaging;

public class UserCreatedConsumer : IConsumer<UserCreatedEvent>
{
    private readonly ILogger<UserCreatedConsumer> _logger;
    public UserCreatedConsumer(ILogger<UserCreatedConsumer> logger) { _logger = logger; }
    public Task Consume(ConsumeContext<UserCreatedEvent> context)
    {
        _logger.LogInformation("User created: {UserId} - {Username} ({Email})", context.Message.UserId, context.Message.Username, context.Message.Email);
        return Task.CompletedTask;
    }
}

public class UserDeactivatedConsumer : IConsumer<UserDeactivatedEvent>
{
    private readonly ILogger<UserDeactivatedConsumer> _logger;
    public UserDeactivatedConsumer(ILogger<UserDeactivatedConsumer> logger) { _logger = logger; }
    public Task Consume(ConsumeContext<UserDeactivatedEvent> context)
    {
        _logger.LogInformation("User deactivated: {UserId} - {Username}", context.Message.UserId, context.Message.Username);
        return Task.CompletedTask;
    }
}
