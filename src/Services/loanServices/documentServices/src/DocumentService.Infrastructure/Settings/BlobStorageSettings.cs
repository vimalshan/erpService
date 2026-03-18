namespace DocumentService.Infrastructure.Settings;

public class BlobStorageSettings
{
    public string ConnectionString { get; set; } = string.Empty;
    public string ContainerName { get; set; } = "loan-documents";
}
