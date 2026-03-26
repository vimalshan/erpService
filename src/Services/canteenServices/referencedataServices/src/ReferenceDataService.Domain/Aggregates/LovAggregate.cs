using ReferenceDataService.Domain.Common;
using ReferenceDataService.Domain.Entities;

namespace ReferenceDataService.Domain.Aggregates;

public class LovAggregate : AggregateRoot
{
    public LovTypeMaster LovTypeMaster { get; private set; } = null!;
    public IReadOnlyCollection<LovMaster> LovMasters => _lovMasters.AsReadOnly();

    private readonly List<LovMaster> _lovMasters = new();

    private LovAggregate() { }

    public LovAggregate(LovTypeMaster lovTypeMaster)
    {
        LovTypeMaster = lovTypeMaster ?? throw new ArgumentNullException(nameof(lovTypeMaster));
    }

    public void AddLovMaster(LovMaster lovMaster)
    {
        if (lovMaster.LovType != LovTypeMaster.LovTypeCode)
            throw new InvalidOperationException("LOV type mismatch.");

        _lovMasters.Add(lovMaster);
    }

    public void RemoveLovMaster(string lovId)
    {
        var item = _lovMasters.FirstOrDefault(x => x.LovId == lovId);
        if (item != null)
            _lovMasters.Remove(item);
    }
}
