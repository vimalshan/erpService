using Azure.Storage.Blobs;
using Microsoft.Azure.Functions.Worker;
using Microsoft.Extensions.Logging;
using System.Text;
using System.Text.Json;

namespace AuthProvider.Functions;

/// <summary>
/// Azure Function – triggered when a new user is created (via Service Bus / queue message).
/// Sends a welcome email (stub) and uploads a welcome blob record to Azure Blob Storage.
/// Demonstrates: Azure Functions, Blob Storage, message queue consumer.
/// </summary>
public sealed class UserCreatedFunction
{
    private readonly ILogger<UserCreatedFunction> _logger;
    private readonly BlobServiceClient _blobServiceClient;

    public UserCreatedFunction(ILogger<UserCreatedFunction> logger, BlobServiceClient blobServiceClient)
    {
        _logger = logger;
        _blobServiceClient = blobServiceClient;
    }

    [Function("UserCreatedFunction")]
    public async Task Run(
        [ServiceBusTrigger("auth.events.usercreatedevent", Connection = "ServiceBusConnection")] string messageBody,
        FunctionContext context)
    {
        _logger.LogInformation("UserCreatedFunction triggered. Message: {Body}", messageBody);

        UserCreatedMessage? message;
        try
        {
            message = JsonSerializer.Deserialize<UserCreatedMessage>(messageBody,
                new JsonSerializerOptions { PropertyNameCaseInsensitive = true });
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "Failed to deserialize UserCreated message.");
            return;
        }

        if (message is null) return;

        // 1. Simulate sending welcome email
        _logger.LogInformation("Sending welcome email to {Email} for user {UserId}", message.Email, message.UserId);

        // 2. Upload user record to Blob Storage
        var containerClient = _blobServiceClient.GetBlobContainerClient("auth-user-events");
        await containerClient.CreateIfNotExistsAsync();

        var blobName = $"users/{message.UserId}/created-{DateTime.UtcNow:yyyyMMddHHmmss}.json";
        var blobClient = containerClient.GetBlobClient(blobName);
        var content = JsonSerializer.Serialize(message);
        using var stream = new MemoryStream(Encoding.UTF8.GetBytes(content));
        await blobClient.UploadAsync(stream, overwrite: true);

        _logger.LogInformation("User creation event archived to blob {BlobName}", blobName);
    }
}

public record UserCreatedMessage(Guid UserId, string Email, string Username, DateTime OccurredOn);
