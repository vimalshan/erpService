namespace PurchaseSalesService.Domain.Entities;

/// <summary>Maps to SALE_SUB table (line items for a sale).</summary>
public sealed class SaleSub
{
    public long? ReferenceNumber { get; private set; }   // SS_REF_NUM
    public long? SerialNumber { get; private set; }      // SS_SER_NUM (FK → SALE_MAIN)
    public string? ProductCode { get; private set; }     // SS_PRO_COD
    public decimal? ProductQuantity { get; private set; }// SS_PRO_QTN
    public string? ProductGrade { get; private set; }    // SS_PRO_GRD
    public string? UserComment { get; private set; }     // SS_USR_COM
    public string? CheckbookInvoice { get; private set; }// SS_CHB_INV
    public char? CancelFlag { get; private set; }        // SS_CAN_FLG

    private SaleSub() { } // EF Core

    public static SaleSub Create(
        long saleSerialNumber,
        string productCode,
        decimal quantity,
        string? productGrade = null,
        string? userComment = null,
        string? checkbookInvoice = null)
    {
        if (quantity < 0) throw new ArgumentException("Quantity cannot be negative.", nameof(quantity));
        return new SaleSub
        {
            SerialNumber = saleSerialNumber,
            ProductCode = productCode,
            ProductQuantity = quantity,
            ProductGrade = productGrade,
            UserComment = userComment,
            CheckbookInvoice = checkbookInvoice,
            CancelFlag = 'N'
        };
    }
}
