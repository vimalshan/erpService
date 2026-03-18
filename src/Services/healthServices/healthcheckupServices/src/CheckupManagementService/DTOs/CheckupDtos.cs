namespace CheckupManagementService.DTOs;

/// <summary>
/// Checkup Master DTO for API responses
/// </summary>
public class CheckupMasterDto
{
    public string CheckupMasterId { get; set; } = string.Empty;
    public string EmployeeNumber { get; set; } = string.Empty;
    public string CheckupType { get; set; } = string.Empty;
    public DateTime CheckupDate { get; set; }
    public string? DoctorCode { get; set; }
    public string? DoctorRemarks { get; set; }
    public string? Status { get; set; }
    public string? ApprovedBy { get; set; }
    public DateTime? ApprovedDate { get; set; }
    public DateTime CreatedOn { get; set; }
}

/// <summary>
/// Create Checkup DTO for input
/// </summary>
public class CreateCheckupDto
{
    public string EmployeeNumber { get; set; } = string.Empty;
    public string CheckupType { get; set; } = string.Empty;
    public DateTime CheckupDate { get; set; }
    public string? DoctorCode { get; set; }
    public List<string> TestIds { get; set; } = new();
}

/// <summary>
/// Update Checkup DTO
/// </summary>
public class UpdateCheckupDto
{
    public string? Status { get; set; }
    public string? DoctorRemarks { get; set; }
    public string? ApprovedBy { get; set; }
}

/// <summary>
/// Health Main DTO for examination results
/// </summary>
public class HealthMainDto
{
    public string HealthId { get; set; } = string.Empty;
    public string CheckupMasterId { get; set; } = string.Empty;
    public string EmployeeNumber { get; set; } = string.Empty;
    public decimal? Height { get; set; }
    public decimal? Weight { get; set; }
    public decimal? BMI { get; set; }
    public decimal? BloodPressure { get; set; }
    public int? HeartRate { get; set; }
    public string? BloodGroup { get; set; }
    public string? EyeVision { get; set; }
    public string? OverallFitness { get; set; }
    public string? MedicalClearance { get; set; }
    public DateTime CreatedOn { get; set; }
}

/// <summary>
/// Create Health Examination DTO
/// </summary>
public class CreateHealthExaminationDto
{
    public string CheckupMasterId { get; set; } = string.Empty;
    public string EmployeeNumber { get; set; } = string.Empty;
    public decimal? Height { get; set; }
    public decimal? Weight { get; set; }
    public decimal? BloodPressure { get; set; }
    public int? HeartRate { get; set; }
    public string? BloodGroup { get; set; }
    public string? EyeVision { get; set; }
    public string? ColorBlindness { get; set; }
    public string? Hearing { get; set; }
    public List<HealthTestResultDto> TestResults { get; set; } = new();
}

/// <summary>
/// Health Test Result DTO
/// </summary>
public class HealthTestResultDto
{
    public string TestName { get; set; } = string.Empty;
    public string? TestValue { get; set; }
    public string? Result { get; set; }
    public string? Remarks { get; set; }
}

/// <summary>
/// Test Master DTO
/// </summary>
public class TestMasterDto
{
    public string TestId { get; set; } = string.Empty;
    public string TestName { get; set; } = string.Empty;
    public string? TestCategory { get; set; }
    public string? NormalRange { get; set; }
    public string? Unit { get; set; }
    public decimal? Cost { get; set; }
    public bool IsActive { get; set; }
}

/// <summary>
/// Create Test Master DTO
/// </summary>
public class CreateTestMasterDto
{
    public string TestName { get; set; } = string.Empty;
    public string? TestCategory { get; set; }
    public string? NormalRange { get; set; }
    public string? Unit { get; set; }
    public decimal? Cost { get; set; }
}

/// <summary>
/// Checkup Others DTO
/// </summary>
public class CheckupOthersDto
{
    public string CheckupOthersId { get; set; } = string.Empty;
    public string CheckupMasterId { get; set; } = string.Empty;
    public string? MedicineAllergy { get; set; }
    public string? FamilyHistory { get; set; }
    public string? PastSurgery { get; set; }
    public string? CurrentMedicines { get; set; }
    public string? LifestyleHabits { get; set; }
    public string? OtherComments { get; set; }
}

/// <summary>
/// Create Checkup Others DTO
/// </summary>
public class CreateCheckupOthersDto
{
    public string CheckupMasterId { get; set; } = string.Empty;
    public string? MedicineAllergy { get; set; }
    public string? FamilyHistory { get; set; }
    public string? PastSurgery { get; set; }
    public string? CurrentMedicines { get; set; }
    public string? LifestyleHabits { get; set; }
    public string? OtherComments { get; set; }
}

/// <summary>
/// Health Check Card DTO
/// </summary>
public class HealthCheckCardDto
{
    public string CardNumber { get; set; } = string.Empty;
    public string CheckupMasterId { get; set; } = string.Empty;
    public string EmployeeNumber { get; set; } = string.Empty;
    public DateTime IssueDate { get; set; }
    public DateTime? ExpiryDate { get; set; }
    public string? CardStatus { get; set; }
    public string? IssuedBy { get; set; }
}

/// <summary>
/// Pagination Result DTO
/// </summary>
public class CheckupPaginationResultDto<T>
{
    public List<T> Data { get; set; } = new();
    public int TotalCount { get; set; }
    public int PageNumber { get; set; }
    public int PageSize { get; set; }
    public int TotalPages => (TotalCount + PageSize - 1) / PageSize;
    public bool HasNextPage => PageNumber < TotalPages;
    public bool HasPreviousPage => PageNumber > 1;
}
