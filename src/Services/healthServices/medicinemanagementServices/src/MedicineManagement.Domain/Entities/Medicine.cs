using MedicineManagement.Domain.Common;

namespace MedicineManagement.Domain.Entities;

public class Medicine : AuditableEntity, IAggregateRoot
{
    public string MedicineCode { get; private set; } = null!;
    public string MedicineName { get; private set; } = null!;
    public string MedicineTypeCode { get; private set; } = null!;
    public char? Category { get; private set; }
    public decimal? OrderLevelMin { get; private set; }
    public decimal? OrderLevelMax { get; private set; }

    // Navigation
    public MedicineType? MedicineType { get; private set; }

    private Medicine() { }

    public static Medicine Create(
        string medicineCode, string medicineName, string medicineTypeCode,
        char? category, decimal? orderMin, decimal? orderMax,
        string entryUser, decimal? userPin)
    {
        var entity = new Medicine
        {
            MedicineCode = medicineCode,
            MedicineName = medicineName,
            MedicineTypeCode = medicineTypeCode,
            Category = category,
            OrderLevelMin = orderMin,
            OrderLevelMax = orderMax,
            EntryUser = entryUser,
            EntryUserPin = userPin,
            EntryDate = DateTime.UtcNow
        };
        entity.AddDomainEvent(new Events.MedicineCreatedEvent(entity));
        return entity;
    }

    public void Update(string medicineName, string medicineTypeCode, char? category,
        decimal? orderMin, decimal? orderMax, string modifiedUser, decimal? modifiedUserPin)
    {
        MedicineName = medicineName;
        MedicineTypeCode = medicineTypeCode;
        Category = category;
        OrderLevelMin = orderMin;
        OrderLevelMax = orderMax;
        ModifiedUser = modifiedUser;
        ModifiedUserPin = modifiedUserPin;
        ModifiedDate = DateTime.UtcNow;
    }

    public bool IsBelowMinimumStock(long currentStock) => OrderLevelMin.HasValue && currentStock < (long)OrderLevelMin.Value;
}
