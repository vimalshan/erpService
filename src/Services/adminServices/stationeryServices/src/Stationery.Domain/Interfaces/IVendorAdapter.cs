namespace Stationery.Domain.Interfaces;

public interface IVendorAdapter
{
    Task<bool> SubmitOrderAsync(long orderId, List<VendorItemDto> items);
}

public record VendorItemDto(string SKU, int Quantity, decimal Price);
