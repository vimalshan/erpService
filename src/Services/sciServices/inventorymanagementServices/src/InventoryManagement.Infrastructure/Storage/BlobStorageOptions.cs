namespace InventoryManagement.Infrastructure.Storage;

public sealed class BlobStorageOptions
{
    public const string Section = "BlobStorage";

    public string ConnectionString { get; set; } = "UseDevelopmentStorage=true";
    public string ItemImagesContainer { get; set; } = "item-images";
}
