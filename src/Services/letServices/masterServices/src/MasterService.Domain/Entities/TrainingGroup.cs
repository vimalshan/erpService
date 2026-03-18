using MasterService.Domain.Common;

namespace MasterService.Domain.Entities;

/// <summary>Reference: TRAIN_GROUP</summary>
public sealed class TrainingGroup : AggregateRoot
{
    public long GroupCode { get; private set; }
    public string? GroupName { get; private set; }

    private TrainingGroup() { }

    public static TrainingGroup Create(long groupCode, string? groupName)
    {
        if (groupCode <= 0) throw new ArgumentException("GroupCode must be positive.");
        return new TrainingGroup { GroupCode = groupCode, GroupName = groupName?.Trim() };
    }
}
