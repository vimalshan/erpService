using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.Logging;

namespace TeamServices.Infrastructure.Messaging.Consumers;

public class TeamCreatedMessage
{
    public long TeamId { get; set; }
    public string TeamName { get; set; } = string.Empty;
}

public class TeamCreatedConsumer : RabbitMqConsumerBase<TeamCreatedMessage>
{
    private readonly ILogger<TeamCreatedConsumer> _logger;

    public TeamCreatedConsumer(IConfiguration configuration, ILogger<TeamCreatedConsumer> logger)
        : base(configuration, logger, "team.created.queue", "team.events", "team.created")
    {
        _logger = logger;
    }

    protected override Task HandleMessageAsync(TeamCreatedMessage message, CancellationToken cancellationToken)
    {
        _logger.LogInformation("Consumed TeamCreated event: TeamId={TeamId}, Name={TeamName}", message.TeamId, message.TeamName);
        // Add business logic here (e.g., send notifications, update caches)
        return Task.CompletedTask;
    }
}

public class TeamMemberChangedMessage
{
    public long TeamId { get; set; }
    public long EmployeeSysId { get; set; }
    public string Action { get; set; } = string.Empty;
}

public class TeamMemberChangedConsumer : RabbitMqConsumerBase<TeamMemberChangedMessage>
{
    private readonly ILogger<TeamMemberChangedConsumer> _logger;

    public TeamMemberChangedConsumer(IConfiguration configuration, ILogger<TeamMemberChangedConsumer> logger)
        : base(configuration, logger, "team.member.changed.queue", "team.events", "team.member.*")
    {
        _logger = logger;
    }

    protected override Task HandleMessageAsync(TeamMemberChangedMessage message, CancellationToken cancellationToken)
    {
        _logger.LogInformation("Consumed TeamMemberChanged event: TeamId={TeamId}, EmpId={EmpId}, Action={Action}",
            message.TeamId, message.EmployeeSysId, message.Action);
        return Task.CompletedTask;
    }
}
