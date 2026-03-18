using Dapper;
using Microsoft.Azure.Functions.Worker;
using Microsoft.Data.SqlClient;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.Logging;
using RabbitMQ.Client;
using System.Text;
using System.Text.Json;

namespace AuditService.Functions.Functions;

/// <summary>
/// Runs every 6 hours to check escalation requirements and publish escalation events.
/// </summary>
public class EscalationFunction
{
    private readonly IConfiguration _configuration;
    private readonly ILogger<EscalationFunction> _logger;

    public EscalationFunction(IConfiguration configuration, ILogger<EscalationFunction> logger)
    {
        _configuration = configuration;
        _logger = logger;
    }

    [Function(nameof(EscalationFunction))]
    public async Task Run([TimerTrigger("0 0 */6 * * *")] TimerInfo timerInfo, CancellationToken cancellationToken)
    {
        _logger.LogInformation("EscalationFunction triggered at {UtcNow}", DateTime.UtcNow);

        var connectionString = _configuration.GetConnectionString("DefaultConnection")
            ?? throw new InvalidOperationException("DefaultConnection not configured.");

        const string sql = @"
            SELECT ao.OBV_ID, ao.OBV_TITLE, ao.OBV_RISK,
                   ao.OBV_ORGDUEDATE, ao.OBV_AUDITEE, ao.OBV_ESC1, ao.OBV_ESC2,
                   am.AUDIT_NAME, am.AUDIT_UNIT,
                   DATEDIFF(DAY, ao.OBV_ORGDUEDATE, GETDATE()) AS DaysOverdue
            FROM AUDIT_OBSERVATION ao
            INNER JOIN AUDIT_MASTER am ON ao.OBV_AUDITID = am.AUDIT_ID
            WHERE ao.OBV_STATUS = 'P'
              AND ao.OBV_ORGDUEDATE < DATEADD(DAY, -7, GETDATE())  -- 7+ days overdue
              AND ao.OBV_APPSTATUS IS NULL
            ORDER BY DaysOverdue DESC";

        using var connection = new SqlConnection(connectionString);
        var escalations = (await connection.QueryAsync(sql)).ToList();

        if (escalations.Count == 0)
        {
            _logger.LogInformation("No escalations required.");
            return;
        }

        _logger.LogWarning("{Count} observations require escalation.", escalations.Count);
        await PublishEscalationEventsAsync(escalations, cancellationToken);
    }

    private async Task PublishEscalationEventsAsync(IEnumerable<dynamic> escalations, CancellationToken cancellationToken)
    {
        var factory = new ConnectionFactory
        {
            HostName = _configuration["RabbitMQ:Host"] ?? "localhost",
            Port = int.Parse(_configuration["RabbitMQ:Port"] ?? "5672"),
            UserName = _configuration["RabbitMQ:Username"] ?? "guest",
            Password = _configuration["RabbitMQ:Password"] ?? "guest"
        };

        try
        {
            using var connection = factory.CreateConnection();
            using var channel = connection.CreateModel();
            channel.ExchangeDeclare("audit.events", ExchangeType.Topic, durable: true);

            foreach (var esc in escalations)
            {
                var message = new
                {
                    ObvId = esc.OBV_ID,
                    Title = esc.OBV_TITLE,
                    Risk = esc.OBV_RISK,
                    DueDate = esc.OBV_ORGDUEDATE,
                    DaysOverdue = esc.DaysOverdue,
                    AuditName = esc.AUDIT_NAME,
                    Esc1 = esc.OBV_ESC1,
                    Esc2 = esc.OBV_ESC2,
                    EscalatedAt = DateTime.UtcNow
                };

                var body = Encoding.UTF8.GetBytes(JsonSerializer.Serialize(message));
                var props = channel.CreateBasicProperties();
                props.Persistent = true;
                props.ContentType = "application/json";
                channel.BasicPublish("audit.events", "observation.escalated", props, body);
            }
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "Failed to publish escalation events.");
        }

        await Task.CompletedTask;
    }
}
