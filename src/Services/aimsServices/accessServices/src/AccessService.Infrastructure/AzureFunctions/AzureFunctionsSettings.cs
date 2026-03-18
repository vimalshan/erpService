namespace AccessService.Infrastructure.AzureFunctions
{
    /// <summary>
    /// Azure Functions configuration settings
    /// </summary>
    public class AzureFunctionsSettings
    {
        public string ConnectionString { get; set; }
        public string QueueName { get; set; }
        public string FunctionAppBaseUrl { get; set; }
        public int MaxRetries { get; set; } = 3;
        public int TimeoutSeconds { get; set; } = 60;
    }
}
