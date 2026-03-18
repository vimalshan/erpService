using MediatR;
using LeaveServices.Domain.Events;
using LeaveServices.Infrastructure.Messaging;
using Microsoft.Extensions.Logging;
using Microsoft.Extensions.Options;

namespace LeaveServices.Infrastructure.EventHandlers;

public sealed class LeaveAppliedEventHandler : INotificationHandler<LeaveAppliedEvent>
{
    private readonly ILogger<LeaveAppliedEventHandler> _logger;
    private readonly RabbitMqSettings _settings;

    public LeaveAppliedEventHandler(ILogger<LeaveAppliedEventHandler> logger, IOptions<RabbitMqSettings> opts)
    {
        _logger   = logger;
        _settings = opts.Value;
    }

    public async Task Handle(LeaveAppliedEvent notification, CancellationToken ct)
    {
        _logger.LogInformation("Domain event: LeaveApplied – DetailId={Id}, Emp={Emp}",
            notification.LeaveDetailId, notification.EmpSysId);

        await using var publisher = await RabbitMqPublisher.CreateAsync(_settings);
        await publisher.PublishAsync("", "leave.applied", notification);
    }
}

public sealed class LeaveApprovedEventHandler : INotificationHandler<LeaveApprovedEvent>
{
    private readonly ILogger<LeaveApprovedEventHandler> _logger;
    private readonly RabbitMqSettings _settings;

    public LeaveApprovedEventHandler(ILogger<LeaveApprovedEventHandler> logger, IOptions<RabbitMqSettings> opts)
    {
        _logger   = logger;
        _settings = opts.Value;
    }

    public async Task Handle(LeaveApprovedEvent notification, CancellationToken ct)
    {
        _logger.LogInformation("Domain event: LeaveApproved – DetailId={Id}", notification.LeaveDetailId);

        await using var publisher = await RabbitMqPublisher.CreateAsync(_settings);
        await publisher.PublishAsync("", "leave.approved", notification);
    }
}

public sealed class LeaveRejectedEventHandler : INotificationHandler<LeaveRejectedEvent>
{
    private readonly ILogger<LeaveRejectedEventHandler> _logger;

    public LeaveRejectedEventHandler(ILogger<LeaveRejectedEventHandler> logger) => _logger = logger;

    public Task Handle(LeaveRejectedEvent notification, CancellationToken ct)
    {
        _logger.LogInformation("Domain event: LeaveRejected – DetailId={Id}, Remarks={R}",
            notification.LeaveDetailId, notification.Remarks);
        return Task.CompletedTask;
    }
}

public sealed class LeaveCancelledEventHandler : INotificationHandler<LeaveCancelledEvent>
{
    private readonly ILogger<LeaveCancelledEventHandler> _logger;

    public LeaveCancelledEventHandler(ILogger<LeaveCancelledEventHandler> logger) => _logger = logger;

    public Task Handle(LeaveCancelledEvent notification, CancellationToken ct)
    {
        _logger.LogInformation("Domain event: LeaveCancelled – DetailId={Id}", notification.LeaveDetailId);
        return Task.CompletedTask;
    }
}
