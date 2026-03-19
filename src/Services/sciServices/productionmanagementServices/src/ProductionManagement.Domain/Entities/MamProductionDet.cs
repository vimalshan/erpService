using ProductionManagement.Domain.Common;

namespace ProductionManagement.Domain.Entities;

public class MamProductionDet : BaseEntity
{
    public int? Id { get; private set; }
    public long? ProductionNo { get; private set; }
    public DateTime? ProductionDate { get; private set; }
    public int? ProductionFg { get; private set; }
    public decimal? ProductionQty { get; private set; }

    private MamProductionDet() { }

    public MamProductionDet(long? productionNo, DateTime? productionDate, int? productionFg, decimal? productionQty)
    {
        ProductionNo = productionNo;
        ProductionDate = productionDate;
        ProductionFg = productionFg;
        ProductionQty = productionQty;
    }
}
