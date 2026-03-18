using MasterService.Domain.Common;

namespace MasterService.Domain.Entities;

/// <summary>Reference: CAT_MAST</summary>
public sealed class Category : AggregateRoot
{
    public string CategoryCode { get; private set; } = string.Empty;
    public string CategoryName { get; private set; } = string.Empty;
    public long? SerialNumber { get; private set; }

    private Category() { }

    public static Category Create(string categoryCode, string categoryName, long? serialNumber = null)
    {
        ArgumentException.ThrowIfNullOrWhiteSpace(categoryCode);
        ArgumentException.ThrowIfNullOrWhiteSpace(categoryName);

        return new Category
        {
            CategoryCode = categoryCode.Trim().ToUpper(),
            CategoryName = categoryName.Trim(),
            SerialNumber = serialNumber
        };
    }

    public void Update(string categoryName) => CategoryName = categoryName.Trim();
}
