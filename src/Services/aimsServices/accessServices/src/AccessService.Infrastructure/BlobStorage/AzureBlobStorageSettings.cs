namespace AccessService.Infrastructure.BlobStorage
{
    /// <summary>
    /// Azure Blob Storage configuration settings
    /// </summary>
    public class AzureBlobStorageSettings
    {
        public string ConnectionString { get; set; }
        public string ContainerName { get; set; }
    }
}
