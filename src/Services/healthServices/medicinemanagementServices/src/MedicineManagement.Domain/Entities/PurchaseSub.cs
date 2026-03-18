using MedicineManagement.Domain.Common;

namespace MedicineManagement.Domain.Entities;

public class PurchaseSub : AuditableEntity
{
    public string CompanyCode { get; private set; } = null!;
    public long TransactionNumber { get; private set; }
    public string SerialNumber { get; private set; } = null!;
    public string MedicineCode { get; private set; } = null!;
    public string PackagingType { get; private set; } = null!;
    public long? PackagingQuantity { get; private set; }
    public long? PackagingNos { get; private set; }
    public long? TotalQuantity { get; private set; }
    public DateTime? ManufacturingDate { get; private set; }
    public DateTime? ExpiryDate { get; private set; }
    public string? LotNumber { get; private set; }
    public char CancelFlag { get; private set; }

    // Navigation
    public PurchaseMain? PurchaseMain { get; private set; }

    private PurchaseSub() { }

    public static PurchaseSub Create(
        string companyCode, long transactionNumber, string serialNumber,
        string medicineCode, string packagingType, long? packagingQuantity,
        long? packagingNos, long? totalQuantity,
        DateTime? manufacturingDate, DateTime? expiryDate, string? lotNumber,
        string entryUser, decimal entryUserPin)
    {
        return new PurchaseSub
        {
            CompanyCode = companyCode,
            TransactionNumber = transactionNumber,
            SerialNumber = serialNumber,
            MedicineCode = medicineCode,
            PackagingType = packagingType,
            PackagingQuantity = packagingQuantity,
            PackagingNos = packagingNos,
            TotalQuantity = totalQuantity,
            ManufacturingDate = manufacturingDate,
            ExpiryDate = expiryDate,
            LotNumber = lotNumber,
            CancelFlag = 'N',
            EntryUser = entryUser,
            EntryUserPin = entryUserPin,
            EntryDate = DateTime.UtcNow
        };
    }

    public bool IsExpired() => ExpiryDate.HasValue && ExpiryDate.Value < DateTime.UtcNow;
}
