using CanteenUnit.Domain.Common;
using CanteenUnit.Domain.Entities;

namespace CanteenUnit.Domain.Aggregates;

/// <summary>
/// Aggregate root that owns a CanteenUnitMaster and its associated accesses.
/// </summary>
public class CanteenUnitAggregate : BaseAggregateRoot
{
    public CanteenUnitMaster Unit { get; private set; } = null!;
    public IReadOnlyCollection<CanteenUnitAccess> ActiveAccesses =>
        Unit.Accesses.Where(a => a.UnClsDat is null).ToList().AsReadOnly();

    private CanteenUnitAggregate() { }

    public static CanteenUnitAggregate Create(
        decimal comCode, string unitName, string? unitRef,
        decimal? maxVal, decimal? minVal, long? siteId, long? hrmsId)
    {
        var aggregate = new CanteenUnitAggregate
        {
            Unit = CanteenUnitMaster.Create(comCode, unitName, unitRef, maxVal, minVal, siteId, hrmsId),
            Version = 1
        };
        return aggregate;
    }

    public void GrantAccess(long accNum, long userId, long enteredBy)
    {
        var access = CanteenUnitAccess.Grant(accNum, (long)Unit.UnComCod, userId, enteredBy);
        Unit.Accesses.Add(access);
        Version++;
    }

    public void RevokeAccess(long accessNum)
    {
        var access = Unit.Accesses.FirstOrDefault(a => a.UnUntAcc == accessNum && a.UnClsDat is null)
            ?? throw new InvalidOperationException($"Active access {accessNum} not found.");
        access.Revoke();
        Version++;
    }
}
