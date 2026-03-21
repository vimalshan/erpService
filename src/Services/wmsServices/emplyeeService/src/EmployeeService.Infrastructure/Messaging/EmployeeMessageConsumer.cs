using System.Text;
using System.Text.Json;
using RabbitMQ.Client;
using RabbitMQ.Client.Events;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.Hosting;
using Microsoft.Extensions.Logging;
using Microsoft.Extensions.DependencyInjection;
using MediatR;

namespace EmployeeService.Infrastructure.Messaging;

public class EmployeeMessageConsumer : BackgroundService
{
    private readonly IConfiguration _configuration;
    private readonly ILogger<EmployeeMessageConsumer> _logger;
    private readonly IServiceProvider _serviceProvider;
    private IConnection? _connection;
    private IChannel? _channel;

    public EmployeeMessageConsumer(
        IConfiguration configuration,
        ILogger<EmployeeMessageConsumer> logger,
        IServiceProvider serviceProvider)
    {
        _configuration = configuration;
        _logger = logger;
        _serviceProvider = serviceProvider;
    }

    protected override async Task ExecuteAsync(CancellationToken stoppingToken)
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

            await _channel.ExchangeDeclareAsync(
                exchange: "employee",
                type: ExchangeType.Topic,
                durable: true,
                cancellationToken: stoppingToken);

            var queueDeclareResult = await _channel.QueueDeclareAsync(
                queue: "employee-service-queue",
                durable: true,
                exclusive: false,
                autoDelete: false,
                cancellationToken: stoppingToken);

            await _channel.QueueBindAsync(
                queue: queueDeclareResult.QueueName,
                exchange: "employee",
                routingKey: "employee.*",
                cancellationToken: stoppingToken);

            var consumer = new AsyncEventingBasicConsumer(_channel);
            consumer.ReceivedAsync += async (_, ea) =>
            {
                try
                {
                    var body = Encoding.UTF8.GetString(ea.Body.ToArray());
                    _logger.LogInformation("Received message: {RoutingKey} - {Body}", ea.RoutingKey, body);

                    await ProcessMessageAsync(ea.RoutingKey, body);

                    await _channel.BasicAckAsync(ea.DeliveryTag, multiple: false);
                }
                catch (Exception ex)
                {
                    _logger.LogError(ex, "Error processing message");
                    await _channel.BasicNackAsync(ea.DeliveryTag, multiple: false, requeue: true);
                }
            };

            await _channel.BasicConsumeAsync(
                queue: queueDeclareResult.QueueName,
                autoAck: false,
                consumer: consumer,
                cancellationToken: stoppingToken);

            _logger.LogInformation("Employee message consumer started");

            await Task.Delay(Timeout.Infinite, stoppingToken);
        }
        catch (OperationCanceledException) when (stoppingToken.IsCancellationRequested)
        {
            // Graceful shutdown
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "Employee message consumer encountered an error");
        }
    }

    private async Task ProcessMessageAsync(string routingKey, string messageBody)
    {
        using var scope = _serviceProvider.CreateScope();
        var logger = scope.ServiceProvider.GetRequiredService<ILogger<EmployeeMessageConsumer>>();

        switch (routingKey)
        {
            case "employee.created":
                logger.LogInformation("Processing employee created event: {Message}", messageBody);
                break;
            case "employee.updated":
                logger.LogInformation("Processing employee updated event: {Message}", messageBody);
                break;
            case "employee.deactivated":
                logger.LogInformation("Processing employee deactivated event: {Message}", messageBody);
                break;
            default:
                logger.LogWarning("Unknown routing key: {RoutingKey}", routingKey);
                break;
        }

        await Task.CompletedTask;
    }

    public override async Task StopAsync(CancellationToken cancellationToken)
    {
        if (_channel is not null)
        {
            await _channel.CloseAsync(cancellationToken);
            _channel.Dispose();
        }
        if (_connection is not null)
        {
            await _connection.CloseAsync(cancellationToken);
            _connection.Dispose();
        }
        await base.StopAsync(cancellationToken);
    }
}
