using MasterDataService.Domain.Common;

namespace MasterDataService.Domain.Aggregates;

/// <summary>
/// Aggregate root for LOV management: groups LovMaster + LovTypeMaster
/// </summary>
public class LovAggregate : BaseEntity<string>
{
    public string TypeCode { get; private set; } = null!;
    public string TypeName { get; private set; } = null!;

    private readonly List<LovEntry> _entries = [];
    public IReadOnlyCollection<LovEntry> Entries => _entries.AsReadOnly();

    private LovAggregate() { }

    public static LovAggregate Create(string typeCode, string typeName)
    {
        return new LovAggregate
        {
            Id = typeCode,
            TypeCode = typeCode,
            TypeName = typeName
        };
    }

    public void AddEntry(long lovId, string lovName)
    {
        _entries.Add(new LovEntry(lovId, lovName));
    }
}

public record LovEntry(long LovId, string LovName);
