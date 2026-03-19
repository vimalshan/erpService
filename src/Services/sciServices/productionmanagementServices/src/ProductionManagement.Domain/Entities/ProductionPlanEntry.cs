using ProductionManagement.Domain.Common;

namespace ProductionManagement.Domain.Entities;

public class ProductionPlanEntry : BaseEntity
{
    public int? Id { get; private set; }
    public string? OracleCode { get; private set; }
    public string? Month { get; private set; }
    public char? ProType { get; private set; }
    public int? ProValue { get; private set; }
    public int? FactoryId { get; private set; }
    public string? Zone { get; private set; }
    public int? ProYear { get; private set; }

    private ProductionPlanEntry() { }

    public ProductionPlanEntry(string? oracleCode, string? month, char? proType, int? proValue, int? factoryId, string? zone, int? proYear)
    {
        OracleCode = oracleCode;
        Month = month;
        ProType = proType;
        ProValue = proValue;
        FactoryId = factoryId;
        Zone = zone;
        ProYear = proYear;
    }
}
