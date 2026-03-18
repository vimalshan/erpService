namespace Todos.Infrastructure.MessageBrokers;

/// <summary>
/// Configuration for Azure Blob Storage
/// </summary>
public class BlobStorageConfiguration
{
    public string? ConnectionString { get; set; }
    public string? ContainerName { get; set; } = "learning-items";
    public string[]? AllowedExtensions { get; set; } = [".jpg", ".jpeg", ".png", ".pdf", ".docx", ".xlsx"];
    public int MaxFileSizeInMB { get; set; } = 10;
}
