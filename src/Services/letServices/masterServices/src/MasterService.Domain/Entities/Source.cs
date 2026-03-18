using MasterService.Domain.Common;

namespace MasterService.Domain.Entities;

/// <summary>Reference: SOURCE_MAST</summary>
public sealed class Source : AggregateRoot
{
    public string SourceCode { get; private set; } = string.Empty;
    public string SourceName { get; private set; } = string.Empty;

    private Source() { }

    public static Source Create(string sourceCode, string sourceName)
    {
        ArgumentException.ThrowIfNullOrWhiteSpace(sourceCode);
        ArgumentException.ThrowIfNullOrWhiteSpace(sourceName);
        return new Source { SourceCode = sourceCode.Trim().ToUpper(), SourceName = sourceName.Trim() };
    }
}
