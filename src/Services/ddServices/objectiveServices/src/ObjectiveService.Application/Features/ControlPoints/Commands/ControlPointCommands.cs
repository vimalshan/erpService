using ObjectiveService.Application.Common;

namespace ObjectiveService.Application.Features.ControlPoints.Commands;

public class CreateControlPointCommand : CommandBase<CommandResult<decimal>>
{
    public decimal EmployeeSysId { get; set; }
    public decimal DDYearId { get; set; }
    public string Source { get; set; }
    public decimal RefId { get; set; }
    public decimal SerialNumber { get; set; }
    public string Description { get; set; }
    public string Category { get; set; }
    public string UnitOfMeasurement { get; set; }
    public string UnitFrom { get; set; }
    public string UnitTo { get; set; }
    public decimal VersionNumber { get; set; }
    public decimal? Weightage { get; set; }
    public decimal? AccountabilityId { get; set; }
}

public class UpdateControlPointCommand : CommandBase<CommandResult>
{
    public decimal Id { get; set; }
    public string Description { get; set; }
    public string UnitFrom { get; set; }
    public string UnitTo { get; set; }
    public decimal? Weightage { get; set; }
}

public class DeleteControlPointCommand : CommandBase<CommandResult>
{
    public decimal Id { get; set; }
}
