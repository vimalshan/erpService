using System.Text;
using System.Text.Json;
using BusServices.Domain.Events;
using Microsoft.Extensions.Logging;
using RabbitMQ.Client;
using RabbitMQ.Client.Events;

namespace BusServices.Infrastructure.Messaging.RabbitMQ.Consumers;

public abstract class BaseConsumer<T> : IDisposable
{
    protected readonly ILogger Logger;
    private IConnection? _connection;
    private IChannel? _channel;

    protected BaseConsumer(ILogger logger) => Logger = logger;

    protected async Task StartConsuming(string queueName, Func<T, Task> handler, ConnectionFactory factory, CancellationToken ct)
    {
        _connection = await factory.CreateConnectionAsync(ct);
        _channel = await _connection.CreateChannelAsync(cancellationToken: ct);
        await _channel.QueueDeclareAsync(queueName, durable: true, exclusive: false, autoDelete: false, cancellationToken: ct);

        var consumer = new AsyncEventingBasicConsumer(_channel);
        consumer.ReceivedAsync += async (_, ea) =>
        {
            try
            {
                var body = Encoding.UTF8.GetString(ea.Body.ToArray());
                var message = JsonSerializer.Deserialize<T>(body);
                if (message is not null) await handler(message);
                await _channel.BasicAckAsync(ea.DeliveryTag, false, ct);
            }
            catch (Exception ex)
            {
                Logger.LogError(ex, "Error processing message from {Queue}", queueName);
                await _channel.BasicNackAsync(ea.DeliveryTag, false, requeue: false, ct);
            }
        };

        await _channel.BasicConsumeAsync(queueName, autoAck: false, consumer, ct);
    }

    public void Dispose()
    {
        _channel?.CloseAsync().GetAwaiter().GetResult();
        _connection?.CloseAsync().GetAwaiter().GetResult();
    }
}

public sealed class BusRegisteredConsumer : BaseConsumer<BusRegisteredEvent>
{
    private readonly string _queueName;

    public BusRegisteredConsumer(string queueName, ILogger<BusRegisteredConsumer> logger) : base(logger)
    {
        _queueName = queueName;
    }

    public Task StartAsync(ConnectionFactory factory, CancellationToken ct)
        => StartConsuming(_queueName, HandleAsync, factory, ct);

    private Task HandleAsync(BusRegisteredEvent evt)
    {
        Logger.LogInformation("Consumer: Bus registered - Id={BusId}, RegNum={RegNumber}", evt.BusId, evt.RegistrationNumber);
        return Task.CompletedTask;
    }
}

public sealed class EmployeeAssignedConsumer : BaseConsumer<EmployeeAssignedToBusEvent>
{
    private readonly string _queueName;

    public EmployeeAssignedConsumer(string queueName, ILogger<EmployeeAssignedConsumer> logger) : base(logger)
    {
        _queueName = queueName;
    }

    public Task StartAsync(ConnectionFactory factory, CancellationToken ct)
        => StartConsuming(_queueName, HandleAsync, factory, ct);

    private Task HandleAsync(EmployeeAssignedToBusEvent evt)
    {
        Logger.LogInformation("Consumer: Employee {EmpSysId} assigned to Bus {BusId}", evt.EmpSysId, evt.BusId);
        return Task.CompletedTask;
    }
}
