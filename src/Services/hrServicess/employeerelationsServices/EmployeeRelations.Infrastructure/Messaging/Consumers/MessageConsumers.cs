using RabbitMQ.Client;
using Microsoft.Extensions.Logging;

namespace EmployeeRelations.Infrastructure.Messaging.Consumers;

public record DisciplinaryActionNotification(long MainId, long ActionId, long EmpSysId, DateTime ActionDate, string ActionType);
public record EwsCompletedNotification(long EwsId, long EmpSysId, int PeriodNo, string FinalFlag);
public record SurveyResponseNotification(long ResponseId, long SurveyId, long EmpSysId);

public class DisciplinaryActionConsumer : RabbitMqConsumerBase<DisciplinaryActionNotification>
{
    private readonly ILogger<DisciplinaryActionConsumer> _log;
    protected override string QueueName => "disciplinary.actions";
    protected override string RoutingKey => "disciplinary.action";

    public DisciplinaryActionConsumer(IConnection connection, ILogger<DisciplinaryActionConsumer> logger)
        : base(connection, logger) => _log = logger;

    protected override Task HandleAsync(DisciplinaryActionNotification message, CancellationToken ct)
    {
        _log.LogInformation("Disciplinary action received: EmpId={EmpId}, ActionId={ActionId}, Date={Date}",
            message.EmpSysId, message.ActionId, message.ActionDate);
        // Trigger notifications, update audit log, etc.
        return Task.CompletedTask;
    }
}

public class EwsCompletedConsumer : RabbitMqConsumerBase<EwsCompletedNotification>
{
    private readonly ILogger<EwsCompletedConsumer> _log;
    protected override string QueueName => "ews.completed";
    protected override string RoutingKey => "ews.completed";

    public EwsCompletedConsumer(IConnection connection, ILogger<EwsCompletedConsumer> logger)
        : base(connection, logger) => _log = logger;

    protected override Task HandleAsync(EwsCompletedNotification message, CancellationToken ct)
    {
        _log.LogInformation("EWS completed: EwsId={EwsId}, EmpId={EmpId}, Period={Period}, Flag={Flag}",
            message.EwsId, message.EmpSysId, message.PeriodNo, message.FinalFlag);
        return Task.CompletedTask;
    }
}

public class SurveyResponseConsumer : RabbitMqConsumerBase<SurveyResponseNotification>
{
    private readonly ILogger<SurveyResponseConsumer> _log;
    protected override string QueueName => "survey.responses";
    protected override string RoutingKey => "survey.response";

    public SurveyResponseConsumer(IConnection connection, ILogger<SurveyResponseConsumer> logger)
        : base(connection, logger) => _log = logger;

    protected override Task HandleAsync(SurveyResponseNotification message, CancellationToken ct)
    {
        _log.LogInformation("Survey response received: ResponseId={ResponseId}, SurveyId={SurveyId}", 
            message.ResponseId, message.SurveyId);
        return Task.CompletedTask;
    }
}
