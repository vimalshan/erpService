using StrategicStock.Domain.Common;
using StrategicStock.Domain.Events;
using StrategicStock.Domain.ValueObjects;

namespace StrategicStock.Domain.Entities;

public sealed class StrategicStockEntity : AggregateRoot<int>
{
    public int? CompanyUnitId { get; private set; }
    public int SciItemId { get; private set; }
    public StrategicStockType? StockType { get; private set; }
    public StockQuantity? MaxQty { get; private set; }
    public string? EffectiveDate { get; private set; }
    public string? ClosureDate { get; private set; }
    public int? SciUserIdCreated { get; private set; }
    public DateTime CreationDate { get; private set; }
    public int? SciUserIdModified { get; private set; }
    public string? ModifiedDate { get; private set; }
    public StockQuantity? FilledQty { get; private set; }

    private StrategicStockEntity() { } // EF Core

    public static StrategicStockEntity Create(
        int strategicStockId,
        int sciItemId,
        int? companyUnitId,
        string? stockTypeCode,
        long? maxQty,
        string? effectiveDate,
        int? createdByUserId)
    {
        var entity = new StrategicStockEntity
        {
            Id = strategicStockId,
            SciItemId = sciItemId,
            CompanyUnitId = companyUnitId,
            StockType = stockTypeCode is not null ? StrategicStockType.FromCode(stockTypeCode) : null,
            MaxQty = maxQty.HasValue ? StockQuantity.Create(maxQty.Value) : null,
            EffectiveDate = effectiveDate,
            SciUserIdCreated = createdByUserId,
            CreationDate = DateTime.UtcNow,
            FilledQty = StockQuantity.Zero
        };

        entity.AddDomainEvent(new StockCreatedEvent(
            entity.Id, entity.SciItemId, entity.CompanyUnitId,
            entity.StockType?.Code, maxQty));

        return entity;
    }

    public void Update(long? maxQty, long? filledQty, string? stockTypeCode, int? modifiedByUserId)
    {
        if (!IsActive)
            throw new InvalidOperationException($"Cannot update a closed strategic stock (ID={Id}).");

        if (maxQty.HasValue) MaxQty = StockQuantity.Create(maxQty.Value);
        if (filledQty.HasValue) FilledQty = StockQuantity.Create(filledQty.Value);
        if (stockTypeCode is not null) StockType = StrategicStockType.FromCode(stockTypeCode);

        SciUserIdModified = modifiedByUserId;
        ModifiedDate = DateTime.UtcNow.ToString("yyyy-MM-dd");

        AddDomainEvent(new StockUpdatedEvent(Id, maxQty, filledQty));
    }

    public void Close(int? modifiedByUserId)
    {
        if (!string.IsNullOrEmpty(ClosureDate) && DateTime.TryParse(ClosureDate, out var existing) && existing < DateTime.UtcNow.Date)
            throw new InvalidOperationException($"Strategic stock (ID={Id}) is already closed.");

        var closureDate = DateTime.UtcNow.ToString("yyyy-MM-dd");
        ClosureDate = closureDate;
        SciUserIdModified = modifiedByUserId;
        ModifiedDate = closureDate;

        AddDomainEvent(new StockClosedEvent(Id, closureDate));
    }

    public bool IsActive => string.IsNullOrEmpty(ClosureDate) ||
                            (DateTime.TryParse(ClosureDate, out var d) && d >= DateTime.UtcNow.Date);
}
