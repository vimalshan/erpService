using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Text;
using System.Text.Json;
using System.Threading;
using System.Threading.Tasks;
using Microsoft.Extensions.Logging;
using Microsoft.Extensions.Configuration;
using RabbitMQ.Client;
using MasterData.Application.Services;

#nullable enable

namespace MasterData.Infrastructure.Services
{
    /// <summary>
    /// RabbitMQ message consumer service
    /// </summary>
    public interface IMessageConsumer
    {
        Task StartAsync(CancellationToken cancellationToken = default);
        Task StopAsync();
    }

    /// <summary>
    /// Implementation of RabbitMQ message consumer
    /// </summary>
    public class RabbitMQMessageConsumer : IMessageConsumer, IDisposable
    {
        private readonly IConfiguration _configuration;
        private IConnection? _connection;
        private IModel? _channel;
        private readonly ILogger<RabbitMQMessageConsumer> _logger;

        private const string CompanyUnitExchange = "masterdata.companyunit";
        private const string LocationExchange = "masterdata.location";
        private const string SupplierExchange = "masterdata.supplier";

        public RabbitMQMessageConsumer(IConfiguration configuration, ILogger<RabbitMQMessageConsumer> logger)
        {
            _configuration = configuration;
            _logger = logger;
        }

        public async Task StartAsync(CancellationToken cancellationToken = default)
        {
            try
            {
                await Task.Run(() =>
                {
                    var rabbitSettings = _configuration.GetSection("RabbitMQ");
                    var factory = new ConnectionFactory()
                    {
                        HostName = rabbitSettings["HostName"] ?? "localhost",
                        Port = int.Parse(rabbitSettings["Port"] ?? "5672"),
                        UserName = rabbitSettings["UserName"] ?? "guest",
                        Password = rabbitSettings["Password"] ?? "guest",
                        VirtualHost = rabbitSettings["VirtualHost"] ?? "/",
                        AutomaticRecoveryEnabled = true
                    };

                    _connection = factory.CreateConnection();
                    _channel = _connection.CreateModel();

                    // Declare exchanges
                    _channel.ExchangeDeclare(CompanyUnitExchange, ExchangeType.Topic, durable: true);
                    _channel.ExchangeDeclare(LocationExchange, ExchangeType.Topic, durable: true);
                    _channel.ExchangeDeclare(SupplierExchange, ExchangeType.Topic, durable: true);

                    // Declare queues
                    var cuQueue = _channel.QueueDeclare("masterdata.companyunit.queue", durable: true);
                    var locQueue = _channel.QueueDeclare("masterdata.location.queue", durable: true);
                    var supQueue = _channel.QueueDeclare("masterdata.supplier.queue", durable: true);

                    // Bind queues to exchanges
                    _channel.QueueBind(cuQueue.QueueName, CompanyUnitExchange, "companyunit.*");
                    _channel.QueueBind(locQueue.QueueName, LocationExchange, "location.*");
                    _channel.QueueBind(supQueue.QueueName, SupplierExchange, "supplier.*");

                    _logger.LogInformation("RabbitMQ consumer started successfully");
                }, cancellationToken);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error starting RabbitMQ consumer");
                throw;
            }
        }

        public async Task StopAsync()
        {
            try
            {
                await Task.Run(() =>
                {
                    if (_channel != null && _channel.IsOpen)
                        _channel.Close();

                    if (_connection != null && _connection.IsOpen)
                        _connection.Close();

                    _logger.LogInformation("RabbitMQ consumer stopped");
                });
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error stopping RabbitMQ consumer");
            }
        }

        public void Dispose()
        {
            StopAsync().GetAwaiter().GetResult();
            _channel?.Dispose();
            _connection?.Dispose();
        }
    }

    /// <summary>
    /// Implementation of message publisher
    /// </summary>
    public class RabbitMQMessagePublisher : IMessagePublisher
    {
        private readonly IConfiguration _configuration;
        private IConnection? _connection;
        private IModel? _channel;
        private readonly ILogger<RabbitMQMessagePublisher> _logger;

        private const string CompanyUnitExchange = "masterdata.companyunit";
        private const string LocationExchange = "masterdata.location";
        private const string SupplierExchange = "masterdata.supplier";

        public RabbitMQMessagePublisher(IConfiguration configuration, ILogger<RabbitMQMessagePublisher> logger)
        {
            _configuration = configuration;
            _logger = logger;
            InitializeConnection();
        }

        private void InitializeConnection()
        {
            try
            {
                var rabbitSettings = _configuration.GetSection("RabbitMQ");
                var factory = new ConnectionFactory()
                {
                    HostName = rabbitSettings["HostName"] ?? "localhost",
                    Port = int.Parse(rabbitSettings["Port"] ?? "5672"),
                    UserName = rabbitSettings["UserName"] ?? "guest",
                    Password = rabbitSettings["Password"] ?? "guest",
                    VirtualHost = rabbitSettings["VirtualHost"] ?? "/",
                    AutomaticRecoveryEnabled = true
                };

                _connection = factory.CreateConnection();
                _channel = _connection.CreateModel();

                // Declare exchanges
                _channel.ExchangeDeclare(CompanyUnitExchange, ExchangeType.Topic, durable: true);
                _channel.ExchangeDeclare(LocationExchange, ExchangeType.Topic, durable: true);
                _channel.ExchangeDeclare(SupplierExchange, ExchangeType.Topic, durable: true);

                _logger.LogInformation("RabbitMQ publisher initialized successfully");
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error initializing RabbitMQ connection");
            }
        }

        public async Task PublishCompanyUnitEventAsync<T>(string eventType, T message)
        {
            if (_channel == null || !_channel.IsOpen)
                throw new InvalidOperationException("RabbitMQ channel is not initialized");

            await Task.Run(async () =>
            {
                var json = JsonSerializer.Serialize(message);
                var body = Encoding.UTF8.GetBytes(json);

                _channel.BasicPublish(
                    CompanyUnitExchange,
                    $"companyunit.{eventType}",
                    body: body
                );

                _logger.LogInformation($"Published company unit event: {eventType}");
                await Task.CompletedTask;
            });
        }

        public async Task PublishLocationEventAsync<T>(string eventType, T message)
        {
            if (_channel == null || !_channel.IsOpen)
                throw new InvalidOperationException("RabbitMQ channel is not initialized");

            await Task.Run(async () =>
            {
                var json = JsonSerializer.Serialize(message);
                var body = Encoding.UTF8.GetBytes(json);

                _channel.BasicPublish(
                    LocationExchange,
                    $"location.{eventType}",
                    body: body
                );

                _logger.LogInformation($"Published location event: {eventType}");
                await Task.CompletedTask;
            });
        }

        public async Task PublishSupplierEventAsync<T>(string eventType, T message)
        {
            if (_channel == null || !_channel.IsOpen)
                throw new InvalidOperationException("RabbitMQ channel is not initialized");

            await Task.Run(async () =>
            {
                var json = JsonSerializer.Serialize(message);
                var body = Encoding.UTF8.GetBytes(json);

                _channel.BasicPublish(
                    SupplierExchange,
                    $"supplier.{eventType}",
                    body: body
                );

                _logger.LogInformation($"Published supplier event: {eventType}");
                await Task.CompletedTask;
            });
        }

        public async Task PublishStateEventAsync(string eventType, object eventData)
        {
            if (_channel == null || !_channel.IsOpen)
                throw new InvalidOperationException("RabbitMQ channel is not initialized");

            await Task.Run(async () =>
            {
                var json = JsonSerializer.Serialize(eventData);
                var body = Encoding.UTF8.GetBytes(json);

                _channel.BasicPublish(
                    "masterdata.state",
                    $"state.{eventType}",
                    body: body
                );

                _logger.LogInformation($"Published state event: {eventType}");
                await Task.CompletedTask;
            });
        }

        public async Task PublishCityEventAsync(string eventType, object eventData)
        {
            if (_channel == null || !_channel.IsOpen)
                throw new InvalidOperationException("RabbitMQ channel is not initialized");

            await Task.Run(async () =>
            {
                var json = JsonSerializer.Serialize(eventData);
                var body = Encoding.UTF8.GetBytes(json);

                _channel.BasicPublish(
                    "masterdata.city",
                    $"city.{eventType}",
                    body: body
                );

                _logger.LogInformation($"Published city event: {eventType}");
                await Task.CompletedTask;
            });
        }

        public async Task PublishCompanyUnitEventAsync(string eventType, object eventData)
        {
            await PublishCompanyUnitEventAsync<object>(eventType, eventData);
        }

        public async Task PublishLocationEventAsync(string eventType, object eventData)
        {
            await PublishLocationEventAsync<object>(eventType, eventData);
        }

        public async Task PublishSupplierEventAsync(string eventType, object eventData)
        {
            await PublishSupplierEventAsync<object>(eventType, eventData);
        }
    }
}
