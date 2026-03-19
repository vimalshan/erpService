using Azure.Storage.Blobs;
using Microsoft.Azure.Functions.Worker;
using Microsoft.Extensions.Logging;

namespace UserService.AzureFunctions.Functions;

/// <summary>
/// Azure Function for processing user events
/// </summary>
public class UserEventProcessor
{
    private readonly ILogger<UserEventProcessor> _logger;

    public UserEventProcessor(ILogger<UserEventProcessor> logger)
    {
        _logger = logger;
    }

    [Function("ProcessUserEvent")]
    public async Task Run(
        [QueueTrigger("user-events-queue", Connection = "AzureWebJobsStorage")] string message,
        FunctionContext context)
    {
        _logger.LogInformation("Processing user event: {Message}", message);

        try
        {
            // Parse and process the event
            _logger.LogInformation("User event processed successfully");
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "Error processing user event");
            throw;
        }
    }
}

/// <summary>
/// Azure Function for uploading user profile image to blob storage
/// </summary>
public class UserProfileImageUploader
{
    private readonly BlobContainerClient _blobContainerClient;
    private readonly ILogger<UserProfileImageUploader> _logger;

    public UserProfileImageUploader(
        BlobContainerClient blobContainerClient,
        ILogger<UserProfileImageUploader> logger)
    {
        _blobContainerClient = blobContainerClient;
        _logger = logger;
    }

    [Function("UploadUserProfileImage")]
    public async Task Run(
        [HttpTrigger(AuthorizationLevel.Anonymous, "post", Route = "upload-profile-image/{userId}")] HttpRequest req,
        long userId,
        FunctionContext context)
    {
        _logger.LogInformation("Uploading profile image for user: {UserId}", userId);

        try
        {
            if (req.Form.Files.Count == 0)
            {
                req.HttpContext.Response.StatusCode = 400;
                await req.HttpContext.Response.WriteAsJsonAsync(new { error = "No file uploaded" });
                return;
            }

            var file = req.Form.Files[0];
            var blobName = $"profile-images/{userId}/{Guid.NewGuid()}-{file.FileName}";

            using (var stream = file.OpenReadStream())
            {
                await _blobContainerClient.UploadBlobAsync(blobName, stream, overwrite: true);
            }

            _logger.LogInformation("Profile image uploaded successfully for user: {UserId}", userId);
            await req.HttpContext.Response.WriteAsJsonAsync(new { blobName });
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "Error uploading profile image");
            req.HttpContext.Response.StatusCode = 500;
            await req.HttpContext.Response.WriteAsJsonAsync(new { error = ex.Message });
        }
    }
}

/// <summary>
/// Azure Function for daily user status report
/// </summary>
public class UserStatusReportFunction
{
    private readonly ILogger<UserStatusReportFunction> _logger;

    public UserStatusReportFunction(ILogger<UserStatusReportFunction> logger)
    {
        _logger = logger;
    }

    [Function("GenerateUserStatusReport")]
    public async Task Run(
        [TimerTrigger("0 0 0 * * *")] TimerInfo myTimer) // Runs daily at midnight
    {
        _logger.LogInformation("User status report generation started at: {Time}", DateTime.UtcNow);

        try
        {
            // Generate report logic
            _logger.LogInformation("User status report generated successfully");
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "Error generating user status report");
            throw;
        }

        if (myTimer. isPastDue)
        {
            _logger.LogWarning("Timer schedule status: overdue");
        }
    }
}
