namespace AccessService.Infrastructure.AzureFunctions
{
    /// <summary>
    /// Model for passing data to Azure Functions
    /// </summary>
    public class AccessServiceFunctionInput
    {
        public string FunctionName { get; set; }
        public string EmployeeSystemId { get; set; }
        public string RoleId { get; set; }
        public string BlobName { get; set; }
        public string EventType { get; set; }
        public DateTime Timestamp { get; set; } = DateTime.UtcNow;
    }
}
