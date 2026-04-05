namespace CompensationService.Application.DTOs;

/// <summary>
/// DTO for creating a compensation grade
/// </summary>
public class CreateCompensationGradeDto
{
    public string GradeCode { get; set; } = null!;
    public string GradeName { get; set; } = null!;
    public int GradeLevel { get; set; }
    public decimal BaseSalary { get; set; }
    public decimal? HraPercentage { get; set; }
    public decimal? DaPercentage { get; set; }
    public DateTime EffectiveFrom { get; set; }
}

/// <summary>
/// DTO for updating a compensation grade
/// </summary>
public class UpdateCompensationGradeDto
{
    public long GradeId { get; set; }
    public string GradeName { get; set; } = null!;
    public decimal BaseSalary { get; set; }
    public decimal? HraPercentage { get; set; }
    public decimal? DaPercentage { get; set; }
}

/// <summary>
/// DTO for compensation grade response
/// </summary>
public class CompensationGradeDto
{
    public long GradeId { get; set; }
    public string GradeCode { get; set; } = null!;
    public string GradeName { get; set; } = null!;
    public int GradeLevel { get; set; }
    public decimal BaseSalary { get; set; }
    public decimal? HraPercentage { get; set; }
    public decimal? DaPercentage { get; set; }
    public decimal? CalculatedHRA { get; set; }
    public decimal? CalculatedDA { get; set; }
    public decimal? TotalSalary { get; set; }
    public string Status { get; set; } = null!;
    public DateTime EffectiveFrom { get; set; }
    public DateTime? EffectiveTo { get; set; }
    public long CreatedBy { get; set; }
    public DateTime CreatedOn { get; set; }
    public long? UpdatedBy { get; set; }
    public DateTime? UpdatedOn { get; set; }
}

/// <summary>
/// DTO for changing grade status
/// </summary>
public class ChangeGradeStatusDto
{
    public long GradeId { get; set; }
    public string NewStatus { get; set; } = null!;
}
