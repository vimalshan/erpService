namespace AccessService.Infrastructure.AzureFunctions
{
    /// <summary>
    /// Interface for Azure Functions integration
    /// Provides methods to queue background tasks
    /// </summary>
    public interface IAzureFunctionsService
    {
        /// <summary>
        /// Queue a background task for processing user role assignment
        /// </summary>
        Task QueueUserRoleAssignmentAsync(long employeeSystemId, int roleId);

        /// <summary>
        /// Queue a background task for processing user access revocation
        /// </summary>
        Task QueueUserAccessRevocationAsync(long employeeSystemId, int roleId);

        /// <summary>
        /// Queue a background task for report generation
        /// </summary>
        Task QueueReportGenerationAsync(string reportType, Dictionary<string, object> parameters);

        /// <summary>
        /// Queue a background task for sending notifications
        /// </summary>
        Task QueueNotificationAsync(string userId, string message, string notificationType = "email");

        /// <summary>
        /// Check if Azure Functions service is connected
        /// </summary>
        Task<bool> IsConnectedAsync();
    }
}
