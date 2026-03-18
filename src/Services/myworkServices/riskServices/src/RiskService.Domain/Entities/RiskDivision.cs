using RiskService.Domain.Common;

namespace RiskService.Domain.Entities;

public class RiskDivision : BaseEntity
{
    public string Name { get; set; } = default!;
    public long HrmsBusinessId { get; set; }

    private readonly List<RiskDivisionUnit> _units = new();
    public IReadOnlyCollection<RiskDivisionUnit> Units => _units.AsReadOnly();

    private readonly List<RiskDivisionFunctionMap> _functionMaps = new();
    public IReadOnlyCollection<RiskDivisionFunctionMap> FunctionMaps => _functionMaps.AsReadOnly();
}
