using EligibilityService.Domain.Common;
using EligibilityService.Domain.Entities;
using EligibilityService.Domain.Events;

namespace EligibilityService.Domain.Aggregates;

/// <summary>
/// Aggregate root that manages the lifecycle of EligibilityMaster entries
/// along with their history snapshots.
/// </summary>
public class EligibilityAggregate : BaseEntity
{
    private readonly List<EligibilityMasterHistory> _historySnapshots = new();

    public EligibilityMaster Master { get; private set; } = default!;
    public IReadOnlyCollection<EligibilityMasterHistory> HistorySnapshots => _historySnapshots.AsReadOnly();

    private EligibilityAggregate() { }

    public static EligibilityAggregate CreateNew(
        long canteenUnit,
        string shiftCode,
        decimal itemCode,
        int? eligibleLimit,
        long? enteredUser,
        string? timeOfficeUnit)
    {
        var aggregate = new EligibilityAggregate();
        aggregate.Master = EligibilityMaster.Create(
            canteenUnit, shiftCode, itemCode, eligibleLimit, enteredUser, timeOfficeUnit);
        return aggregate;
    }

    public static EligibilityAggregate Load(EligibilityMaster master)
    {
        return new EligibilityAggregate { Master = master };
    }

    public void UpdateLimit(int? newLimit, string? timeOfficeUnit, long modifiedUser)
    {
        // Take a history snapshot before the update
        var snapshot = EligibilityMasterHistory.CreateFrom(Master, modifiedUser);
        _historySnapshots.Add(snapshot);
        Master.Update(newLimit, timeOfficeUnit, modifiedUser);
    }

    public bool IsEligible(int requestedQty) =>
        Master.EligibleLimit.HasValue && Master.EligibleLimit.Value >= requestedQty;
}
