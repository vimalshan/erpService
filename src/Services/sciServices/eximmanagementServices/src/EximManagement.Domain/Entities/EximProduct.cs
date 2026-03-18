using EximManagement.Domain.Common;
using EximManagement.Domain.Events;

namespace EximManagement.Domain.Entities;

/// <summary>Represents a registered EXIM product in the master.</summary>
public class EximProduct : BaseEntity
{
    public long ProductId { get; private set; }
    public string ProductName { get; private set; } = default!;
    public string? ProductOracleCode { get; private set; }
    public long LastUpdatedBy { get; private set; }
    public DateTime LastUpdatedOn { get; private set; }
    public char Status { get; private set; }

    private readonly List<EximProductSearch> _searches = new();
    public IReadOnlyCollection<EximProductSearch> Searches => _searches.AsReadOnly();

    private EximProduct() { }

    public static EximProduct Create(long productId, string productName, string? oracleCode, long updatedBy)
    {
        if (string.IsNullOrWhiteSpace(productName))
            throw new ArgumentException("Product name is required.", nameof(productName));

        var product = new EximProduct
        {
            ProductId = productId,
            ProductName = productName,
            ProductOracleCode = oracleCode,
            LastUpdatedBy = updatedBy,
            LastUpdatedOn = DateTime.UtcNow,
            Status = 'Y'
        };

        product.AddDomainEvent(new EximProductCreatedEvent(product.ProductId, product.ProductName, DateTime.UtcNow));
        return product;
    }

    public void Update(string productName, string? oracleCode, long updatedBy)
    {
        ProductName = productName;
        ProductOracleCode = oracleCode;
        LastUpdatedBy = updatedBy;
        LastUpdatedOn = DateTime.UtcNow;
    }

    public void Deactivate(long updatedBy)
    {
        Status = 'N';
        LastUpdatedBy = updatedBy;
        LastUpdatedOn = DateTime.UtcNow;
    }

    public void AddSearch(EximProductSearch search) => _searches.Add(search);
}
