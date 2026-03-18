using MasterService.Domain.Common;

namespace MasterService.Domain.Entities;

/// <summary>Reference: SKILL_GROUP</summary>
public sealed class SkillGroup : AggregateRoot
{
    public string GroupCode { get; private set; } = string.Empty;
    public string GroupName { get; private set; } = string.Empty;

    private SkillGroup() { }

    public static SkillGroup Create(string groupCode, string groupName)
    {
        ArgumentException.ThrowIfNullOrWhiteSpace(groupCode);
        ArgumentException.ThrowIfNullOrWhiteSpace(groupName);
        return new SkillGroup { GroupCode = groupCode.Trim().ToUpper(), GroupName = groupName.Trim() };
    }
}
