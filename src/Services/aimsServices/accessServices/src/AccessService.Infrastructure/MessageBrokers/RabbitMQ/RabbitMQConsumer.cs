using System;
using System.Text.Json;
using System.Threading.Tasks;
using Microsoft.Extensions.Logging;
using RabbitMQ.Client;
using RabbitMQ.Client.Events;
using AccessService.Infrastructure.DomainEvents;

namespace AccessService.Infrastructure.MessageBrokers.RabbitMQ
{
    /// <summary>
    /// Base class for RabbitMQ event consumers
    /// Handles message consumption, deserialization, and handler invocation
    /// </summary>
    public abstract class RabbitMQConsumer
    {
        protected readonly IRabbitMQConnection _connection;
        protected readonly ILogger _logger;
        protected readonly IdempotencyService _idempotencyService;
        protected IModel _channel;

        protected abstract string QueueName { get; }
        protected abstract string ExchangeName { get; }
        protected abstract string RoutingKey { get; }

        public RabbitMQConsumer(
            IRabbitMQConnection connection,
            IdempotencyService idempotencyService,
            ILogger logger)
        {
            _connection = connection ?? throw new ArgumentNullException(nameof(connection));
            _idempotencyService = idempotencyService ?? throw new ArgumentNullException(nameof(idempotencyService));
            _logger = logger ?? throw new ArgumentNullException(nameof(logger));
        }

        public async Task StartAsync()
        {
            try
            {
                _channel = await _connection.GetChannelAsync();
                
                // Declare exchange and queue
                _channel.ExchangeDeclare(
                    exchange: ExchangeName,
                    type: ExchangeType.Topic,
                    durable: true,
                    autoDelete: false,
                    arguments: null);

                _channel.QueueDeclare(
                    queue: QueueName,
                    durable: true,
                    exclusive: false,
                    autoDelete: false,
                    arguments: null);

                _channel.QueueBind(
                    queue: QueueName,
                    exchange: ExchangeName,
                    routingKey: RoutingKey);

                // Set prefetch count for manual ack
                _channel.BasicQos(0, 1, false);

                // Create consumer
                var consumer = new AsyncEventingBasicConsumer(_channel);
                consumer.Received += Consumer_Received;

                _channel.BasicConsume(
                    queue: QueueName,
                    autoAck: false,
                    consumerTag: $"{GetType().Name}",
                    noLocal: false,
                    exclusive: false,
                    arguments: null,
                    consumer: consumer);

                _logger.LogInformation($"{GetType().Name} started consuming from queue: {QueueName}");
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, $"Error starting {GetType().Name}");
                throw;
            }
        }

        public async Task StopAsync()
        {
            try
            {
                if (_channel != null && _channel.IsOpen)
                {
                    _channel.Close();
                    _channel.Dispose();
                }
                _logger.LogInformation($"{GetType().Name} stopped");
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, $"Error stopping {GetType().Name}");
            }
        }

        private async Task Consumer_Received(object sender, BasicDeliverEventArgs e)
        {
            var messageId = e.BasicProperties?.MessageId ?? Guid.NewGuid().ToString();

            try
            {
                // Check idempotency
                if (!_idempotencyService.IsFirstAttempt(messageId))
                {
                    _logger.LogWarning($"Duplicate message received: {messageId}. Skipping processing.");
                    _channel.BasicAck(e.DeliveryTag, false);
                    return;
                }

                var message = System.Text.Encoding.UTF8.GetString(e.Body.ToArray());
                _logger.LogInformation($"Message received: {messageId}. Content: {message}");

                // Process the message
                await ProcessMessageAsync(message, messageId);

                // Acknowledge successful processing
                _channel.BasicAck(e.DeliveryTag, false);
                _logger.LogInformation($"Message processed successfully: {messageId}");
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, $"Error processing message: {messageId}");
                
                // Negative acknowledgment to requeue the message
                _channel.BasicNack(e.DeliveryTag, false, true);
            }
        }

        protected abstract Task ProcessMessageAsync(string message, string messageId);
    }
}
