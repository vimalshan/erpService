namespace AccidentManagementService.DTOs;

/// <summary>
/// Daily Accident FIR DTO for API responses
/// </summary>
public class DailyAccidentFIRDto
{
    public decimal AccidentNumber { get; set; }
    public string? EmployeeNumber { get; set; }
    public string? EmployeeName { get; set; }
    public string? WorkerName { get; set; }
    public string? ContractorId { get; set; }
    public string? ContractorName { get; set; }
    public string? EmployeeDepartment { get; set; }
    public DateTime AccidentDateTime { get; set; }
    public string? AccidentLocation { get; set; }
    public string? NatureOfInjury { get; set; }
    public string? BodyPartAffected { get; set; }
    public string? ShiftName { get; set; }
    public string? MedicalCentreName { get; set; }
    public string? TreatmentGiven { get; set; }
    public DateTime MedicalCentreReceivingDate { get; set; }
    public string CompanyCode { get; set; } = string.Empty;
    public long InjuryCategoryCode { get; set; }
    public long NatureOfInjuryCode { get; set; }
    public string? PreventiveMeasures { get; set; }
    public string? CauseOfIncident { get; set; }
    public string? Status { get; set; }
    public string? Remarks { get; set; }
}

/// <summary>
/// Create Daily Accident FIR DTO
/// </summary>
public class CreateDailyAccidentFIRDto
{
    public string? EmployeeNumber { get; set; }
    public string? EmployeeName { get; set; }
    public string? WorkerName { get; set; }
    public string? ContractorId { get; set; }
    public string? ContractorName { get; set; }
    public string? EmployeeDepartment { get; set; }
    public DateTime AccidentDateTime { get; set; }
    public string? AccidentLocation { get; set; }
    public string? NatureOfInjury { get; set; }
    public string? BodyPartAffected { get; set; }
    public string? ShiftName { get; set; }
    public string? MedicalCentreName { get; set; }
    public string? TreatmentGiven { get; set; }
    public DateTime MedicalCentreReceivingDate { get; set; }
    public string CompanyCode { get; set; } = string.Empty;
    public long InjuryCategoryCode { get; set; }
    public long NatureOfInjuryCode { get; set; }
    public string? PreventiveMeasures { get; set; }
    public string? CauseOfIncident { get; set; }
    public string? ShiftInChargePersonName { get; set; }
}

/// <summary>
/// Update Daily Accident FIR DTO
/// </summary>
public class UpdateDailyAccidentFIRDto
{
    public decimal AccidentNumber { get; set; }
    public string? TreatmentGiven { get; set; }
    public string? PreventiveMeasures { get; set; }
    public string? Status { get; set; }
    public string? Remarks { get; set; }
}

/// <summary>
/// Accident Contractor DTO
/// </summary>
public class AccidentContractorDto
{
    public int Id { get; set; }
    public string? ContractorName { get; set; }
    public decimal ContractorId { get; set; }
    public char Status { get; set; }
}

/// <summary>
/// Injury Category DTO
/// </summary>
public class InjuryCategoryDto
{
    public long CategoryId { get; set; }
    public string? CategoryName { get; set; }
    public string? Description { get; set; }
    public bool IsActive { get; set; }
}

/// <summary>
/// Nature Of Injury DTO
/// </summary>
public class NatureOfInjuryDto
{
    public long NatureId { get; set; }
    public string? NatureName { get; set; }
    public string? Description { get; set; }
    public bool IsActive { get; set; }
}

/// <summary>
/// Doctor/Attendant DTO
/// </summary>
public class DoctorAttendantDto
{
    public long DoctorAttendantId { get; set; }
    public string? Code { get; set; }
    public char Flag { get; set; }
    public string? Name { get; set; }
    public string? Specialization { get; set; }
    public string? ContactNumber { get; set; }
    public bool IsActive { get; set; }
}

/// <summary>
/// Personal Injury DTO
/// </summary>
public class PersonalInjuryDto
{
    public int Id { get; set; }
    public decimal SerialNum { get; set; }
    public string? PersonInjuredName { get; set; }
    public char EmployeeStatus { get; set; }
    public string? Remarks { get; set; }
}

/// <summary>
/// Accident Report DTO (Domain Model)
/// </summary>
public class AccidentReportDto
{
    public long Id { get; set; }
    public Guid Guid { get; set; }
    public long AccidentNumber { get; set; }
    public string CompanyCode { get; set; } = null!;
    
    // Personnel
    public string? EmployeeNumber { get; set; }
    public string? EmployeeName { get; set; }
    public string? Department { get; set; }
    public long? ContractorId { get; set; }
    public string? ContractorName { get; set; }
    
    // Injured Person
    public string InjuredPersonName { get; set; } = null!;
    public long InjuredPersonSerialNumber { get; set; }
    public char InjuredPersonStatus { get; set; }
    
    // Accident Details
    public string AccidentLocation { get; set; } = null!;
    public DateTime AccidentDateTime { get; set; }
    public string BodyPart { get; set; } = null!;
    public string? Shift { get; set; }
    
    // Injury
    public long InjuryCategoryId { get; set; }
    public string? InjuryCategoryName { get; set; }
    public long InjuryNatureId { get; set; }
    public string? InjuryNatureName { get; set; }
    
    // Treatment
    public string MedicalCentreName { get; set; } = null!;
    public DateTime MedicalCentreReceivedDate { get; set; }
    public string TreatmentGiven { get; set; } = null!;
    
    // Status & Severity
    public long SeverityId { get; set; }
    public string? SeverityName { get; set; }
    public long StatusId { get; set; }
    public string? StatusName { get; set; }
    
    // Audit
    public DateTime CreatedDate { get; set; }
    public DateTime? UpdatedDate { get; set; }
    public string? CreatedBy { get; set; }
    public string? UpdatedBy { get; set; }
    public bool IsDeleted { get; set; }
}

/// <summary>
/// Accident Severity DTO
/// </summary>
public class AccidentSeverityDto
{
    public long Id { get; set; }
    public Guid Guid { get; set; }
    public string Code { get; set; } = null!;
    public string Name { get; set; } = null!;
    public string? Description { get; set; }
}

/// <summary>
/// Accident Status DTO
/// </summary>
public class AccidentStatusDto
{
    public long Id { get; set; }
    public Guid Guid { get; set; }
    public string Code { get; set; } = null!;
    public string Name { get; set; } = null!;
    public string? Description { get; set; }
}

/// <summary>
/// Contractor DTO
/// </summary>
public class ContractorDto
{
    public long Id { get; set; }
    public Guid Guid { get; set; }
    public string Name { get; set; } = null!;
    public long ContractorId { get; set; }
    public char Status { get; set; }
}

/// <summary>
/// Injured Person DTO
/// </summary>
public class InjuredPersonDto
{
    public long Id { get; set; }
    public Guid Guid { get; set; }
    public string PersonName { get; set; } = null!;
    public long SerialNumber { get; set; }
    public char EmployeeStatus { get; set; }
}
