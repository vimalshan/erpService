using MasterService.Domain.Common;

namespace MasterService.Domain.Entities;

/// <summary>Reference: FUNCTION_MAST</summary>
public sealed class FunctionMaster : AggregateRoot
{
    public string FunctionCode { get; private set; } = string.Empty;
    public string FunctionName { get; private set; } = string.Empty;
    public string GroupCode { get; private set; } = string.Empty;
    public string UnitCode { get; private set; } = string.Empty;
    public long? SerialNumber { get; private set; }

    private FunctionMaster() { }

    public static FunctionMaster Create(string functionCode, string functionName, string groupCode, string unitCode, long? serialNumber = null)
    {
        ArgumentException.ThrowIfNullOrWhiteSpace(functionCode);
        ArgumentException.ThrowIfNullOrWhiteSpace(functionName);
        return new FunctionMaster
        {
            FunctionCode = functionCode.Trim().ToUpper(),
            FunctionName = functionName.Trim(),
            GroupCode = groupCode.Trim().ToUpper(),
            UnitCode = unitCode.Trim(),
            SerialNumber = serialNumber
        };
    }
}
