using CashManagement.Domain.Common;
using CashManagement.Domain.Events;
using CashManagement.Domain.Exceptions;
using CashManagement.Domain.ValueObjects;

namespace CashManagement.Domain.Entities;

public class CashUnit : AggregateRoot
{
    public string Name { get; private set; } = default!;
    public string Code { get; private set; } = default!;
    public string? Location { get; private set; }
    public long? InChargeEmployeeId { get; private set; }
    public decimal OpeningBalance { get; private set; }
    public EntityStatus Status { get; private set; }
    public long CreatedBy { get; private set; }
    public DateTime CreatedOn { get; private set; }
    public long? UpdatedBy { get; private set; }
    public DateTime? UpdatedOn { get; private set; }

    private readonly List<CashTransaction> _transactions = new();
    public IReadOnlyCollection<CashTransaction> Transactions => _transactions.AsReadOnly();

    private CashUnit() { }

    public static CashUnit Create(long id, string name, string code, string? location,
        long? inChargeEmployeeId, decimal openingBalance, long createdBy)
    {
        var unit = new CashUnit
        {
            Id = id,
            Name = name,
            Code = code,
            Location = location,
            InChargeEmployeeId = inChargeEmployeeId,
            OpeningBalance = openingBalance,
            Status = EntityStatus.Active,
            CreatedBy = createdBy,
            CreatedOn = DateTime.UtcNow
        };
        unit.AddDomainEvent(new CashUnitCreatedEvent(unit.Id, unit.Name));
        return unit;
    }

    public void Deactivate(long updatedBy)
    {
        Status = EntityStatus.Inactive;
        UpdatedBy = updatedBy;
        UpdatedOn = DateTime.UtcNow;
    }

    public void Activate(long updatedBy)
    {
        Status = EntityStatus.Active;
        UpdatedBy = updatedBy;
        UpdatedOn = DateTime.UtcNow;
    }
}
