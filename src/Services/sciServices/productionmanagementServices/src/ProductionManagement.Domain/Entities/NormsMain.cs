using ProductionManagement.Domain.Common;

namespace ProductionManagement.Domain.Entities;

public class NormsMain : BaseEntity, IAggregateRoot
{
    public long NormNo { get; private set; }
    public DateTime NormEffDate { get; private set; }
    public DateTime? NormClsDate { get; private set; }

    // Navigation
    private readonly List<NormsMaster> _normsMasters = new();
    public IReadOnlyCollection<NormsMaster> NormsMasters => _normsMasters.AsReadOnly();

    private NormsMain() { }

    public NormsMain(long normNo, DateTime normEffDate)
    {
        NormNo = normNo;
        NormEffDate = normEffDate;
    }

    public void Close()
    {
        NormClsDate = DateTime.UtcNow;
    }

    public void AddNormsMaster(long normId, int inputCode, int outputCode, int normRate)
    {
        _normsMasters.Add(new NormsMaster(normId, inputCode, outputCode, normRate, NormNo));
    }
}
