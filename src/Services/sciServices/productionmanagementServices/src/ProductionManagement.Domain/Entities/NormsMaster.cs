using ProductionManagement.Domain.Common;

namespace ProductionManagement.Domain.Entities;

public class NormsMaster : BaseEntity
{
    public int? Id { get; private set; }
    public long? NormId { get; private set; }
    public int? NormInputCode { get; private set; }
    public int? NormOutputCode { get; private set; }
    public int? NormRate { get; private set; }
    public long? NormNo { get; private set; }

    // Navigation
    public NormsMain? NormsMain { get; private set; }

    private NormsMaster() { }

    public NormsMaster(long? normId, int? normInputCode, int? normOutputCode, int? normRate, long? normNo)
    {
        NormId = normId;
        NormInputCode = normInputCode;
        NormOutputCode = normOutputCode;
        NormRate = normRate;
        NormNo = normNo;
    }
}
