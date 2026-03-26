namespace AccessService.Infrastructure.MessageBrokers.RabbitMQ
{
    /// <summary>
    /// RabbitMQ configuration settings
    /// </summary>
    public class RabbitMQSettings
    {
        public string Host { get; set; }
        public int Port { get; set; }
        public string Username { get; set; }
        public string Password { get; set; }
        public string VirtualHost { get; set; }
        public string ExchangeName { get; set; } = "access-service-exchange";
        public string QueueName { get; set; } = "access-service-queue";
    }
}
