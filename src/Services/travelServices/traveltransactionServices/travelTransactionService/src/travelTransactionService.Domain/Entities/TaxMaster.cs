using travelTransactionService.Domain.Common;

namespace travelTransactionService.Domain.Entities;

public class TaxMaster : AggregateRoot
{
    public long TaxVendorId { get; private set; }
    public string TaxType { get; private set; } = null!;
    public decimal? TaxRate { get; private set; }
    public DateTime TaxEffectiveDate { get; private set; }
    public DateTime? TaxCloseDate { get; private set; }
    public long? ModifiedBy { get; private set; }
    public DateTime? ModifiedOn { get; private set; }

    private readonly List<TaxComponent> _components = [];
    public IReadOnlyCollection<TaxComponent> Components => _components.AsReadOnly();

    private TaxMaster() { }

    public static TaxMaster Create(
        long vendorId,
        string taxType,
        decimal? taxRate,
        DateTime effectiveDate)
    {
        var tax = new TaxMaster
        {
            TaxVendorId = vendorId,
            TaxType = taxType,
            TaxRate = taxRate,
            TaxEffectiveDate = effectiveDate
        };

        tax.AddDomainEvent(new Events.TaxMasterCreatedEvent(vendorId, taxType));
        return tax;
    }

    public void UpdateRate(decimal newRate, long modifiedBy)
    {
        TaxRate = newRate;
        ModifiedBy = modifiedBy;
        ModifiedOn = DateTime.UtcNow;
    }

    public void Close(DateTime closeDate, long modifiedBy)
    {
        TaxCloseDate = closeDate;
        ModifiedBy = modifiedBy;
        ModifiedOn = DateTime.UtcNow;
    }

    public void AddComponent(TaxComponent component) => _components.Add(component);
}
