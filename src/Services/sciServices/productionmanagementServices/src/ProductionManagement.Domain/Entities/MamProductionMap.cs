using ProductionManagement.Domain.Common;

namespace ProductionManagement.Domain.Entities;

public class MamProductionMap : BaseEntity
{
    public int? Id { get; private set; }
    public int? RmCode { get; private set; }
    public int? FgCode { get; private set; }
    public decimal? SlNo { get; private set; }

    private MamProductionMap() { }

    public MamProductionMap(int? rmCode, int? fgCode, decimal? slNo)
    {
        RmCode = rmCode;
        FgCode = fgCode;
        SlNo = slNo;
    }
}
