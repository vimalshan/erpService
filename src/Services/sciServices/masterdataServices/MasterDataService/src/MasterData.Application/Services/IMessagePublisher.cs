using System.Threading.Tasks;

#nullable enable

namespace MasterData.Application.Services
{
    /// <summary>
    /// Interface for publishing messages to RabbitMQ
    /// </summary>
    public interface IMessagePublisher
    {
        Task PublishCompanyUnitEventAsync(string eventType, object eventData);
        Task PublishLocationEventAsync(string eventType, object eventData);
        Task PublishSupplierEventAsync(string eventType, object eventData);
        Task PublishStateEventAsync(string eventType, object eventData);
        Task PublishCityEventAsync(string eventType, object eventData);
    }
}
