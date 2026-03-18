using System.Text;
using System.Text.Json;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using Microsoft.Extensions.Logging;
using RabbitMQ.Client;
using RabbitMQ.Client.Events;
using TdsService.Application.Files.Commands.UpdateEmailStatus;
using MediatR;

namespace TdsService.Infrastructure.Messaging;

/// <summary>
/// Background service that consumes TDS email confirmation messages from RabbitMQ
/// and marks the corresponding TDS file as email-sent.
/// </summary>
public sealed class TdsEmailConfirmationConsumer : BackgroundService
{
    private const string Exchange = "tds.exchange";
    private const string Queue = "tds.email.confirmation";
    private const string RoutingKey = "tds.email.sent";

    private readonly IServiceScopeFactory _scopeFactory;
    private readonly ILogger<TdsEmailConfirmationConsumer> _logger;
    private readonly string _hostName;
    private readonly string _userName;
    private readonly string _password;

    private IConnection? _connection;
    private IChannel? _channel;

    public TdsEmailConfirmationConsumer(
        IServiceScopeFactory scopeFactory,
        ILogger<TdsEmailConfirmationConsumer> logger,
        string hostName,
        string userName,
        string password)
    {
        _scopeFactory = scopeFactory;
        _logger = logger;
        _hostName = hostName;
        _userName = userName;
        _password = password;
    }

    protected override async Task ExecuteAsync(CancellationToken stoppingToken)
    {
        var factory = new ConnectionFactory
        {
            HostName = _hostName,
            UserName = _userName,
            Password = _password
        };

        try
        {
            _connection = await factory.CreateConnectionAsync(stoppingToken);
            _channel = await _connection.CreateChannelAsync(cancellationToken: stoppingToken);

            await _channel.ExchangeDeclareAsync(Exchange, ExchangeType.Topic, durable: true, cancellationToken: stoppingToken);
            await _channel.QueueDeclareAsync(Queue, durable: true, exclusive: false, autoDelete: false, cancellationToken: stoppingToken);
            await _channel.QueueBindAsync(Queue, Exchange, RoutingKey, cancellationToken: stoppingToken);

            var consumer = new AsyncEventingBasicConsumer(_channel);
            consumer.ReceivedAsync += async (_, ea) =>
            {
                try
                {
                    var body = Encoding.UTF8.GetString(ea.Body.ToArray());
                    var payload = JsonSerializer.Deserialize<EmailConfirmationPayload>(body);

                    if (payload is not null)
                    {
                        using var scope = _scopeFactory.CreateScope();
                        var mediator = scope.ServiceProvider.GetRequiredService<IMediator>();
                        await mediator.Send(new UpdateEmailStatusCommand(payload.FileId), stoppingToken);
                    }

                    await _channel.BasicAckAsync(ea.DeliveryTag, false, stoppingToken);
                }
                catch (Exception ex)
                {
                    _logger.LogError(ex, "Error processing TDS email confirmation message.");
                    await _channel.BasicNackAsync(ea.DeliveryTag, false, requeue: true, cancellationToken: stoppingToken);
                }
            };

            await _channel.BasicConsumeAsync(Queue, autoAck: false, consumer: consumer, cancellationToken: stoppingToken);

            // Keep running until cancellation
            await Task.Delay(Timeout.Infinite, stoppingToken);
        }
        catch (OperationCanceledException) { /* graceful shutdown */ }
        catch (Exception ex)
        {
            _logger.LogError(ex, "TDS EmailConfirmation consumer failed.");
        }
    }

    public override async Task StopAsync(CancellationToken cancellationToken)
    {
        if (_channel is not null) await _channel.DisposeAsync();
        if (_connection is not null) await _connection.DisposeAsync();
        await base.StopAsync(cancellationToken);
    }

    private sealed record EmailConfirmationPayload(long FileId);
}
