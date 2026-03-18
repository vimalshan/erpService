using System;
using System.Collections.Concurrent;
using System.Threading.Tasks;
using Microsoft.Extensions.Logging;
using RabbitMQ.Client;

namespace AccessService.Infrastructure.MessageBrokers.RabbitMQ
{
    /// <summary>
    /// RabbitMQ connection implementation with automatic reconnection and channel pooling
    /// </summary>
    public class RabbitMQConnection : IRabbitMQConnection
    {
        private readonly RabbitMQSettings _settings;
        private readonly ILogger<RabbitMQConnection> _logger;
        private IConnection _connection;
        private readonly ConcurrentBag<IModel> _channels = new();
        private readonly object _connectionLock = new();

        public RabbitMQConnection(RabbitMQSettings settings, ILogger<RabbitMQConnection> logger)
        {
            _settings = settings ?? throw new ArgumentNullException(nameof(settings));
            _logger = logger ?? throw new ArgumentNullException(nameof(logger));
        }

        public async Task ConnectAsync()
        {
            lock (_connectionLock)
            {
                try
                {
                    if (_connection != null && _connection.IsOpen)
                    {
                        _logger.LogInformation("RabbitMQ connection already established");
                        return;
                    }

                    var factory = new ConnectionFactory()
                    {
                        HostName = _settings.Host,
                        Port = _settings.Port,
                        UserName = _settings.Username,
                        Password = _settings.Password,
                        VirtualHost = _settings.VirtualHost,
                        AutomaticRecoveryEnabled = true,
                        DispatchConsumersAsync = true,
                        RequestedHeartbeat = TimeSpan.FromSeconds(30),
                        ContinuationTimeout = TimeSpan.FromSeconds(10)
                    };

                    _connection = factory.CreateConnection();
                    _logger.LogInformation($"RabbitMQ connected to {_settings.Host}:{_settings.Port}");
                }
                catch (Exception ex)
                {
                    _logger.LogError(ex, "Failed to connect to RabbitMQ");
                    throw;
                }
            }
        }

        public async Task DisconnectAsync()
        {
            lock (_connectionLock)
            {
                try
                {
                    // Close all channels
                    while (_channels.TryTake(out var channel))
                    {
                        if (channel != null && channel.IsOpen)
                        {
                            channel.Close();
                            channel.Dispose();
                        }
                    }

                    // Close connection
                    if (_connection != null && _connection.IsOpen)
                    {
                        _connection.Close();
                        _connection.Dispose();
                        _connection = null;
                    }

                    _logger.LogInformation("RabbitMQ connection closed");
                }
                catch (Exception ex)
                {
                    _logger.LogError(ex, "Error disconnecting from RabbitMQ");
                }
            }
        }

        public async Task<bool> IsConnectedAsync()
        {
            lock (_connectionLock)
            {
                return _connection != null && _connection.IsOpen;
            }
        }

        public async Task<IModel> GetChannelAsync()
        {
            lock (_connectionLock)
            {
                if (_connection == null || !_connection.IsOpen)
                {
                    throw new InvalidOperationException("RabbitMQ connection is not established. Call ConnectAsync first.");
                }

                // Try to get a channel from the pool
                if (_channels.TryTake(out var channel))
                {
                    if (channel.IsOpen)
                    {
                        return channel;
                    }
                    else
                    {
                        channel.Dispose();
                    }
                }

                // Create a new channel
                var newChannel = _connection.CreateModel();
                return newChannel;
            }
        }

        public void ReturnChannel(IModel channel)
        {
            if (channel != null && channel.IsOpen)
            {
                _channels.Add(channel);
            }
            else
            {
                channel?.Dispose();
            }
        }
    }
}
