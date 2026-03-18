using MasterService.Domain.Common;

namespace MasterService.Domain.Entities;

/// <summary>Reference: MODE_MAST</summary>
public sealed class Mode : AggregateRoot
{
    public string ModeCode { get; private set; } = string.Empty;
    public string ModeDescription { get; private set; } = string.Empty;

    private Mode() { }

    public static Mode Create(string modeCode, string modeDescription)
    {
        ArgumentException.ThrowIfNullOrWhiteSpace(modeCode);
        ArgumentException.ThrowIfNullOrWhiteSpace(modeDescription);
        return new Mode { ModeCode = modeCode.Trim().ToUpper(), ModeDescription = modeDescription.Trim() };
    }
}
