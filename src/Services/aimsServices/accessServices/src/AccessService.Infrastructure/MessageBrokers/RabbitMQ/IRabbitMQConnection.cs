using RabbitMQ.Client;

namespace AccessService.Infrastructure.MessageBrokers.RabbitMQ
{
    /// <summary>
    /// Interface for RabbitMQ connection management
    /// </summary>
    public interface IRabbitMQConnection
    {
        Task ConnectAsync();
        Task DisconnectAsync();
        Task<bool> IsConnectedAsync();
        Task<IModel> GetChannelAsync();
    }
}
