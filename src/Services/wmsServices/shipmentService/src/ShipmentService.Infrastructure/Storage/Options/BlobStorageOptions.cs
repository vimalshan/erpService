namespace ShipmentService.Infrastructure.Storage.Options;

public sealed class BlobStorageOptions
{
    public const string SectionName = "BlobStorage";
    public string ConnectionString { get; set; } = "UseDevelopmentStorage=true";
    public string ShipmentDocumentsContainer { get; set; } = "shipment-documents";
}
