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
/// Runs daily at 8:00 AM UTC to identify overdue observations and send reminders.
/// </summary>
public class ObservationReminderFunction
{
    private readonly IConfiguration _configuration;
    private readonly ILogger<ObservationReminderFunction> _logger;

    public ObservationReminderFunction(IConfiguration configuration, ILogger<ObservationReminderFunction> logger)
    {
        _configuration = configuration;
        _logger = logger;
    }

    [Function(nameof(ObservationReminderFunction))]
    public async Task Run([TimerTrigger("0 0 8 * * *")] TimerInfo timerInfo, CancellationToken cancellationToken)
    {
        _logger.LogInformation("ObservationReminderFunction triggered at {UtcNow}", DateTime.UtcNow);

        var connectionString = _configuration.GetConnectionString("DefaultConnection")
            ?? throw new InvalidOperationException("DefaultConnection not configured.");

        const string sql = @"
            SELECT ao.OBV_ID, ao.OBV_TITLE, ao.OBV_RISK, ao.OBV_ORGDUEDATE,
                   ao.OBV_AUDITEE, ao.OBV_ESC1, ao.OBV_ESC2, am.AUDIT_NAME
            FROM AUDIT_OBSERVATION ao
            INNER JOIN AUDIT_MASTER am ON ao.OBV_AUDITID = am.AUDIT_ID
            WHERE ao.OBV_STATUS = 'P' AND ao.OBV_ORGDUEDATE < GETDATE()
            ORDER BY ao.OBV_ORGDUEDATE ASC";

        using var connection = new SqlConnection(connectionString);
        var overdueObservations = (await connection.QueryAsync(sql)).ToList();

        if (overdueObservations.Count == 0)
        {
            _logger.LogInformation("No overdue observations found.");
            return;
        }

        _logger.LogInformation("Found {Count} overdue observations.", overdueObservations.Count);
        await PublishReminderMessagesAsync(overdueObservations, cancellationToken);
    }

    private async Task PublishReminderMessagesAsync(IEnumerable<dynamic> observations, CancellationToken cancellationToken)
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

            foreach (var obs in observations)
            {
                var message = new
                {
                    ObvId = obs.OBV_ID,
                    Title = obs.OBV_TITLE,
                    DueDate = obs.OBV_ORGDUEDATE,
                    AuditName = obs.AUDIT_NAME,
                    ReminderSentAt = DateTime.UtcNow
                };

                var body = Encoding.UTF8.GetBytes(JsonSerializer.Serialize(message));
                var props = channel.CreateBasicProperties();
                props.Persistent = true;
                props.ContentType = "application/json";

                channel.BasicPublish("audit.events", "observation.reminder", props, body);
                string obvIdStr = Convert.ToString(obs.OBV_ID) ?? "unknown";
                _logger.LogDebug("Published reminder for observation {ObvId}", obvIdStr);
            }
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "Failed to publish observation reminder messages.");
        }

        await Task.CompletedTask;
    }
}
