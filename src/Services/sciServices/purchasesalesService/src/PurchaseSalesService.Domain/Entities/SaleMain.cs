using PurchaseSalesService.Domain.Common;
using PurchaseSalesService.Domain.Events;

namespace PurchaseSalesService.Domain.Entities;

/// <summary>Maps to SALE_MAIN table.</summary>
public sealed class SaleMain : BaseEntity
{
    public long SerialNumber { get; private set; }       // SL_SER_NUM (PK)
    public long TrackingNumber { get; private set; }     // SL_TRC_NUM
    public long TransactionNumber { get; private set; }  // SL_TRN_NUM
    public long PurposeCode { get; private set; }        // SL_PUR_COD
    public long StageCode { get; private set; }          // SL_STG_COD
    public string? IsoNumber { get; private set; }       // SL_ISO_NUM
    public DateTime? IsoDate { get; private set; }       // SL_ISO_DATE
    public string? ProductDescription { get; private set; } // SL_PRO_DES
    public string UserId { get; private set; } = null!;  // SL_USR_ID
    public long UserNumber { get; private set; }         // SL_USR_NUM
    public DateTime UpdatedAt { get; private set; }      // SL_UPD_DAT
    public char? CancelFlag { get; private set; }        // SL_CAN_FLG
    public string? VehicleCustomer { get; private set; } // SL_VEH_CUS

    private readonly List<SaleSub> _saleSubItems = new();
    public IReadOnlyCollection<SaleSub> SaleSubItems => _saleSubItems.AsReadOnly();

    private SaleMain() { } // EF Core

    public static SaleMain Create(
        long trackingNumber,
        long transactionNumber,
        long purposeCode,
        long stageCode,
        string userId,
        long userNumber,
        string? vehicleCustomer = null)
    {
        var entity = new SaleMain
        {
            TrackingNumber = trackingNumber,
            TransactionNumber = transactionNumber,
            PurposeCode = purposeCode,
            StageCode = stageCode,
            UserId = userId,
            UserNumber = userNumber,
            UpdatedAt = DateTime.UtcNow,
            CancelFlag = 'N',
            VehicleCustomer = vehicleCustomer
        };

        entity.AddDomainEvent(new SaleCreatedEvent(entity));
        return entity;
    }

    public void Cancel(string modifiedBy)
    {
        if (CancelFlag == 'Y') throw new InvalidOperationException("Sale is already cancelled.");
        CancelFlag = 'Y';
        UpdatedAt = DateTime.UtcNow;
        AddDomainEvent(new SaleCancelledEvent(this, modifiedBy));
    }

    public void AddSubItem(SaleSub subItem)
    {
        ArgumentNullException.ThrowIfNull(subItem);
        _saleSubItems.Add(subItem);
    }

    public void SetIsoInfo(string isoNumber, DateTime isoDate)
    {
        IsoNumber = isoNumber;
        IsoDate = isoDate;
        UpdatedAt = DateTime.UtcNow;
    }
}
