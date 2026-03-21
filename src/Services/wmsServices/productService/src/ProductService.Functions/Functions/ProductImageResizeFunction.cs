using Microsoft.Azure.Functions.Worker;
using Microsoft.Extensions.Logging;
using ProductService.Application.Interfaces;

namespace ProductService.Functions;

public class ProductImageResizeFunction(ILogger<ProductImageResizeFunction> logger, IBlobStorageService blobStorage)
{
    [Function("ProductImageResize")]
    public async Task Run(
        [BlobTrigger("product-images/{name}", Connection = "AzureBlobStorage:ConnectionString")] Stream imageStream,
        string name,
        CancellationToken ct)
    {
        logger.LogInformation("Processing image resize for blob: {Name}", name);

        // Placeholder: In production, use an image processing library like SkiaSharp
        // to resize the image and save thumbnails
        var thumbnailName = $"thumbnails/{name}";
        await blobStorage.UploadAsync("product-images", thumbnailName, imageStream, "image/jpeg", ct);

        logger.LogInformation("Created thumbnail for {Name} at {ThumbnailName}", name, thumbnailName);
    }
}
