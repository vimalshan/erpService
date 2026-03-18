namespace ObjectiveService.Application.DTOs;

public class ControlPointDto
{
    public decimal Id { get; set; }
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
    public DateTime? ModifiedDate { get; set; }
    public string Status { get; set; }
}

public class CreateControlPointDto
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

public class UpdateControlPointDto
{
    public decimal Id { get; set; }
    public string Description { get; set; }
    public string UnitFrom { get; set; }
    public string UnitTo { get; set; }
    public decimal? Weightage { get; set; }
}
