using System.Text;
using System.Text.Json;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.Hosting;
using Microsoft.Extensions.Logging;
using RabbitMQ.Client;
using RabbitMQ.Client.Events;

namespace InvestmentService.Infrastructure.Services;

public abstract class RabbitMqConsumerBase : BackgroundService
{
    private IConnection? _connection;
    private IChannel? _channel;
    private readonly IConfiguration _configuration;
    protected readonly ILogger Logger;
    protected abstract string QueueName { get; }
    protected abstract string Exchange { get; }
    protected abstract string RoutingKey { get; }

    protected RabbitMqConsumerBase(IConfiguration configuration, ILogger logger)
    {
        _configuration = configuration;
        Logger = logger;
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

                await _channel.ExchangeDeclareAsync(Exchange, ExchangeType.Topic, durable: true, cancellationToken: stoppingToken);
                await _channel.QueueDeclareAsync(QueueName, durable: true, exclusive: false, autoDelete: false, cancellationToken: stoppingToken);
                await _channel.QueueBindAsync(QueueName, Exchange, RoutingKey, cancellationToken: stoppingToken);

                var consumer = new AsyncEventingBasicConsumer(_channel);
                consumer.ReceivedAsync += async (_, ea) =>
                {
                    try
                    {
                        var body = Encoding.UTF8.GetString(ea.Body.ToArray());
                        await HandleMessageAsync(body, stoppingToken);
                        await _channel.BasicAckAsync(ea.DeliveryTag, false, stoppingToken);
                    }
                    catch (Exception ex)
                    {
                        Logger.LogError(ex, "Error processing message from {Queue}", QueueName);
                        await _channel.BasicNackAsync(ea.DeliveryTag, false, true, stoppingToken);
                    }
                };

                await _channel.BasicConsumeAsync(QueueName, autoAck: false, consumer: consumer, cancellationToken: stoppingToken);

                Logger.LogInformation("RabbitMQ consumer {Queue} started successfully", QueueName);

                while (!stoppingToken.IsCancellationRequested)
                    await Task.Delay(5000, stoppingToken);
            }
            catch (OperationCanceledException) when (stoppingToken.IsCancellationRequested)
            {
                break;
            }
            catch (Exception ex)
            {
                Logger.LogWarning(ex, "RabbitMQ consumer {Queue} failed to connect. Retrying in 30s...", QueueName);
                _channel?.Dispose();
                _connection?.Dispose();
                _channel = null;
                _connection = null;

                try { await Task.Delay(30000, stoppingToken); }
                catch (OperationCanceledException) { break; }
            }
        }
    }

    protected abstract Task HandleMessageAsync(string message, CancellationToken ct);

    public override void Dispose()
    {
        _channel?.Dispose();
        _connection?.Dispose();
        base.Dispose();
    }
}

public class InvestmentMaturityConsumer : RabbitMqConsumerBase
{
    protected override string QueueName => "investment-maturity-queue";
    protected override string Exchange => "investment-events";
    protected override string RoutingKey => "event.investment.matured";

    public InvestmentMaturityConsumer(IConfiguration configuration, ILogger<InvestmentMaturityConsumer> logger)
        : base(configuration, logger) { }

    protected override Task HandleMessageAsync(string message, CancellationToken ct)
    {
        Logger.LogInformation("Processing maturity event: {Message}", message);
        return Task.CompletedTask;
    }
}

public class InvestmentRedemptionConsumer : RabbitMqConsumerBase
{
    protected override string QueueName => "investment-redemption-queue";
    protected override string Exchange => "investment-events";
    protected override string RoutingKey => "event.investment.redeemed";

    public InvestmentRedemptionConsumer(IConfiguration configuration, ILogger<InvestmentRedemptionConsumer> logger)
        : base(configuration, logger) { }

    protected override Task HandleMessageAsync(string message, CancellationToken ct)
    {
        Logger.LogInformation("Processing redemption event: {Message}", message);
        return Task.CompletedTask;
    }
}

public class InvestmentApprovalConsumer : RabbitMqConsumerBase
{
    protected override string QueueName => "investment-approval-queue";
    protected override string Exchange => "investment-events";
    protected override string RoutingKey => "event.investment.approved.#";

    public InvestmentApprovalConsumer(IConfiguration configuration, ILogger<InvestmentApprovalConsumer> logger)
        : base(configuration, logger) { }

    protected override Task HandleMessageAsync(string message, CancellationToken ct)
    {
        Logger.LogInformation("Processing approval event: {Message}", message);
        return Task.CompletedTask;
    }
}
