using PurchaseSalesService.Domain.Common;
using PurchaseSalesService.Domain.Events;

namespace PurchaseSalesService.Domain.Entities;

/// <summary>
/// Maps to PURCHASE_DETAILS table.
/// </summary>
public sealed class PurchaseDetail : BaseEntity
{
    public long SerialNumber { get; private set; }       // PD_SRL_NUM (PK)
    public long TrackingNumber { get; private set; }     // PD_TRC_NUM
    public long TransactionNumber { get; private set; }  // PD_TRN_NUM
    public long PurposeCode { get; private set; }        // PD_PUR_COD
    public long StageCode { get; private set; }          // PD_STG_COD
    public long? OracleMerchandise { get; private set; } // PD_ORA_MRC
    public string? SupplierCode { get; private set; }    // PD_SUP_COD
    public string? TonNumLoaded { get; private set; }    // PD_TON_NUM_LD
    public string? TonNumUnloaded { get; private set; }  // PD_TON_NUM_UD
    public string? UserId { get; private set; }          // PD_USR_ID
    public long? UserNumber { get; private set; }        // PD_USR_NUM
    public DateTime UpdatedAt { get; private set; }      // PD_UPD_DAT
    public char? CancelFlag { get; private set; }        // PD_CAN_FLG

    private PurchaseDetail() { } // EF Core

    public static PurchaseDetail Create(
        long trackingNumber,
        long transactionNumber,
        long purposeCode,
        long stageCode,
        string? supplierCode,
        string userId,
        long userNumber)
    {
        var entity = new PurchaseDetail
        {
            TrackingNumber = trackingNumber,
            TransactionNumber = transactionNumber,
            PurposeCode = purposeCode,
            StageCode = stageCode,
            SupplierCode = supplierCode,
            UserId = userId,
            UserNumber = userNumber,
            UpdatedAt = DateTime.UtcNow,
            CancelFlag = 'N'
        };

        entity.AddDomainEvent(new PurchaseCreatedEvent(entity));
        return entity;
    }

    public void Cancel(string modifiedBy)
    {
        if (CancelFlag == 'Y') throw new InvalidOperationException("Purchase is already cancelled.");
        CancelFlag = 'Y';
        UpdatedAt = DateTime.UtcNow;
        AddDomainEvent(new PurchaseCancelledEvent(this, modifiedBy));
    }

    public void Update(
        string? supplierCode,
        string? tonNumLoaded,
        string? tonNumUnloaded,
        long? oracleMerchandise)
    {
        SupplierCode = supplierCode;
        TonNumLoaded = tonNumLoaded;
        TonNumUnloaded = tonNumUnloaded;
        OracleMerchandise = oracleMerchandise;
        UpdatedAt = DateTime.UtcNow;
    }
}
