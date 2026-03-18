using MasterService.Domain.Common;

namespace MasterService.Domain.Entities;

/// <summary>Reference: FUNCTION_GROUP</summary>
public sealed class FunctionGroup : AggregateRoot
{
    public string GroupCode { get; private set; } = string.Empty;
    public string GroupName { get; private set; } = string.Empty;
    public long? SerialNumber { get; private set; }

    private FunctionGroup() { }

    public static FunctionGroup Create(string groupCode, string groupName, long? serialNumber = null)
    {
        ArgumentException.ThrowIfNullOrWhiteSpace(groupCode);
        ArgumentException.ThrowIfNullOrWhiteSpace(groupName);
        return new FunctionGroup { GroupCode = groupCode.Trim().ToUpper(), GroupName = groupName.Trim(), SerialNumber = serialNumber };
    }
}
