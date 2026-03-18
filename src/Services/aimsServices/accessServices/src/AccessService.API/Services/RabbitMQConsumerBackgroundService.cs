using System;
using System.Collections.Generic;
using System.Threading;
using System.Threading.Tasks;
using Microsoft.Extensions.Hosting;
using Microsoft.Extensions.Logging;
using AccessService.Infrastructure.MessageBrokers.RabbitMQ;
using AccessService.Infrastructure.MessageBrokers.RabbitMQ.Consumers;

namespace AccessService.API.Services
{
    /// <summary>
    /// Background service that manages RabbitMQ event consumer lifecycle
    /// Starts all consumers on application startup and gracefully shuts them down
    /// </summary>
    public class RabbitMQConsumerBackgroundService : BackgroundService
    {
        private readonly IServiceProvider _serviceProvider;
        private readonly ILogger<RabbitMQConsumerBackgroundService> _logger;
        private readonly List<RabbitMQConsumer> _consumers = new();

        public RabbitMQConsumerBackgroundService(
            IServiceProvider serviceProvider,
            ILogger<RabbitMQConsumerBackgroundService> logger)
        {
            _serviceProvider = serviceProvider ?? throw new ArgumentNullException(nameof(serviceProvider));
            _logger = logger ?? throw new ArgumentNullException(nameof(logger));
        }

        protected override async Task ExecuteAsync(CancellationToken stoppingToken)
        {
            _logger.LogInformation("RabbitMQ Consumer Background Service starting");

            try
            {
                // Initialize all consumers
                await InitializeConsumersAsync();

                // Keep the service running until cancellation is requested
                await Task.Delay(Timeout.Infinite, stoppingToken);
            }
            catch (OperationCanceledException)
            {
                _logger.LogInformation("RabbitMQ Consumer Background Service stopping");
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error in RabbitMQ Consumer Background Service");
            }
        }

        private async Task InitializeConsumersAsync()
        {
            try
            {
                // Create consumer instances using the service provider
                var consumers = new RabbitMQConsumer[]
                {
                    (RabbitMQConsumer)_serviceProvider.GetService(typeof(UserMapCreatedEventConsumer)),
                    (RabbitMQConsumer)_serviceProvider.GetService(typeof(UserMapActivatedEventConsumer)),
                    (RabbitMQConsumer)_serviceProvider.GetService(typeof(UserRoleAssignedEventConsumer)),
                    (RabbitMQConsumer)_serviceProvider.GetService(typeof(UserRoleRevokedEventConsumer))
                };

                // Start all non-null consumers
                foreach (var consumer in consumers)
                {
                    if (consumer != null)
                    {
                        try
                        {
                            await consumer.StartAsync();
                            _consumers.Add(consumer);
                        }
                        catch (Exception ex)
                        {
                            _logger.LogError(ex, $"Failed to start consumer: {consumer.GetType().Name}");
                        }
                    }
                }

                _logger.LogInformation($"Initialized {_consumers.Count} RabbitMQ consumers");
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error initializing RabbitMQ consumers");
            }
        }

        public override async Task StopAsync(CancellationToken cancellationToken)
        {
            _logger.LogInformation("Stopping RabbitMQ consumers");

            // Stop all consumers gracefully
            foreach (var consumer in _consumers)
            {
                try
                {
                    await consumer.StopAsync();
                }
                catch (Exception ex)
                {
                    _logger.LogError(ex, $"Error stopping consumer: {consumer.GetType().Name}");
                }
            }

            _consumers.Clear();

            await base.StopAsync(cancellationToken);
        }
    }
}
