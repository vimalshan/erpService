using ReceivingService.Domain.Common;

namespace ReceivingService.Domain.Entities;

/// <summary>
/// Represents a single line of a receiving transaction –
/// one product received into a specific bin.
/// </summary>
public sealed class ReceivingLine : Entity
{
    public int ReceivingId      { get; private set; }
    public int PoLineId         { get; private set; }
    public int ProductId        { get; private set; }
    public int BinId            { get; private set; }
    public decimal QuantityReceived { get; private set; }
    public string? LotNumber    { get; private set; }
    public DateOnly? ExpiryDate { get; private set; }
    public string? Notes        { get; private set; }

    // EF Core navigation
    public Receiving Receiving  { get; private set; } = null!;

    private ReceivingLine() { }

    public static ReceivingLine Create(
        int receivingId,
        int poLineId,
        int productId,
        int binId,
        decimal quantityReceived,
        string? lotNumber = null,
        DateOnly? expiryDate = null,
        string? notes = null)
    {
        if (quantityReceived <= 0)
            throw new ArgumentException("Quantity received must be greater than zero.", nameof(quantityReceived));

        return new ReceivingLine
        {
            ReceivingId      = receivingId,
            PoLineId         = poLineId,
            ProductId        = productId,
            BinId            = binId,
            QuantityReceived = quantityReceived,
            LotNumber        = lotNumber,
            ExpiryDate       = expiryDate,
            Notes            = notes
        };
    }
}
