using System.Text;
using System.Text.Json;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.Hosting;
using Microsoft.Extensions.Logging;
using RabbitMQ.Client;
using RabbitMQ.Client.Events;

namespace HRDocumentService.Infrastructure.Messaging;

public sealed class DocumentApprovalConsumer : BackgroundService
{
    private readonly ILogger<DocumentApprovalConsumer> _logger;
    private readonly IConfiguration _configuration;
    private IConnection? _connection;
    private IChannel? _channel;

    public DocumentApprovalConsumer(IConfiguration configuration, ILogger<DocumentApprovalConsumer> logger)
    {
        _configuration = configuration;
        _logger = logger;
    }

    protected override async Task ExecuteAsync(CancellationToken stoppingToken)
    {
        while (!stoppingToken.IsCancellationRequested)
        {
            try
            {
                var factory = new ConnectionFactory
                {
                    HostName = _configuration["RabbitMQ:HostName"] ?? "localhost",
                    UserName = _configuration["RabbitMQ:UserName"] ?? "guest",
                    Password = _configuration["RabbitMQ:Password"] ?? "guest",
                    Port = int.Parse(_configuration["RabbitMQ:Port"] ?? "5672")
                };

                _connection = await factory.CreateConnectionAsync(stoppingToken);
                _channel = await _connection.CreateChannelAsync(cancellationToken: stoppingToken);

                await _channel.ExchangeDeclareAsync("hr-documents", ExchangeType.Topic, durable: true, cancellationToken: stoppingToken);
                var queueDeclareOk = await _channel.QueueDeclareAsync("hr-document-approvals", durable: true, exclusive: false, autoDelete: false, cancellationToken: stoppingToken);
                await _channel.QueueBindAsync(queueDeclareOk.QueueName, "hr-documents", "document.approved", cancellationToken: stoppingToken);

                var consumer = new AsyncEventingBasicConsumer(_channel);
                consumer.ReceivedAsync += async (_, ea) =>
                {
                    var body = Encoding.UTF8.GetString(ea.Body.Span);
                    _logger.LogInformation("Received document approval event: {Message}", body);

                    // Process approval notification (e.g., send email, update external system)
                    await _channel.BasicAckAsync(ea.DeliveryTag, false);
                };

                await _channel.BasicConsumeAsync(queueDeclareOk.QueueName, autoAck: false, consumer: consumer, cancellationToken: stoppingToken);

                _logger.LogInformation("DocumentApprovalConsumer started listening.");

                await Task.Delay(Timeout.Infinite, stoppingToken);
            }
            catch (OperationCanceledException)
            {
                _logger.LogInformation("DocumentApprovalConsumer is stopping.");
                break;
            }
            catch (Exception ex)
            {
                _logger.LogWarning(ex, "RabbitMQ consumer connection failed. Retrying in 30 seconds...");
                try { await Task.Delay(TimeSpan.FromSeconds(30), stoppingToken); }
                catch (OperationCanceledException) { break; }
            }
        }
    }

    public override async Task StopAsync(CancellationToken ct)
    {
        if (_channel is not null) await _channel.CloseAsync(ct);
        if (_connection is not null) await _connection.CloseAsync(ct);
        await base.StopAsync(ct);
    }
}
