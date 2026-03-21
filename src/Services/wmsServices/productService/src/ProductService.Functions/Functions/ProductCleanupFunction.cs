using Microsoft.Azure.Functions.Worker;
using Microsoft.Extensions.Logging;
using ProductService.Domain.Interfaces;

namespace ProductService.Functions;

public class ProductCleanupFunction(ILogger<ProductCleanupFunction> logger, IProductRepository productRepository)
{
    [Function("ProductCleanup")]
    public async Task Run([TimerTrigger("0 0 2 * * *")] TimerInfo timerInfo, CancellationToken ct)
    {
        logger.LogInformation("ProductCleanup function started at {Time}", DateTime.UtcNow);

        var products = await productRepository.GetAllAsync(ct);
        var inactiveProducts = products.Where(p => !p.IsActive && p.ModifiedDate < DateTime.UtcNow.AddDays(-90)).ToList();

        foreach (var product in inactiveProducts)
        {
            await productRepository.DeleteAsync(product, ct);
            logger.LogInformation("Deleted inactive product {Sku} (inactive since {ModifiedDate})", product.Sku, product.ModifiedDate);
        }

        logger.LogInformation("ProductCleanup completed. Removed {Count} inactive products.", inactiveProducts.Count);
    }
}
