namespace AccidentManagementService.Domain.Entities;

/// <summary>
/// Accident Contractor List Entity
/// </summary>
public class AccidentContractorList
{
    public int Id { get; set; }
    public string? ContractorName { get; set; }
    public decimal ContractorId { get; set; }
    public char Status { get; set; }
    public DateTime CreatedDate { get; set; }
    public DateTime? ModifiedDate { get; set; }
    public string? CreatedBy { get; set; }
    public string? ModifiedBy { get; set; }
}

/// <summary>
/// Personal Injury Entity
/// </summary>
public class PersonalInjury
{
    public int Id { get; set; }
    public decimal SerialNum { get; set; }
    public string? PersonInjuredName { get; set; }
    public char EmployeeStatus { get; set; } // S-SRF/C-CONTRACTOR
    public DateTime CreatedDate { get; set; }
    public DateTime? ModifiedDate { get; set; }
    public string? Remarks { get; set; }
}

/// <summary>
/// Injury Category Master Entity (Legacy)
/// </summary>
public class LegacyInjuryCategoryEntity
{
    public long CategoryId { get; set; }
    public string? CategoryName { get; set; }
    public string? Description { get; set; }
    public bool IsActive { get; set; }
    public DateTime CreatedDate { get; set; }
    public DateTime? ModifiedDate { get; set; }
}

/// <summary>
/// Nature of Injury Master Entity
/// </summary>
public class NatureOfInjury
{
    public long NatureId { get; set; }
    public string? NatureName { get; set; }
    public string? Description { get; set; }
    public bool IsActive { get; set; }
    public DateTime CreatedDate { get; set; }
    public DateTime? ModifiedDate { get; set; }
}

/// <summary>
/// Daily Accident FIR (First Information Report) Entity
/// </summary>
public class DailyAccidentFIR
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
    public string? EnteredUserID { get; set; }
    public decimal EnteredUserNumber { get; set; }
    public DateTime EnteredDate { get; set; }
    public long InjuryCategoryCode { get; set; }
    public long NatureOfInjuryCode { get; set; }
    public string? PreventiveMeasures { get; set; }
    public string? CauseOfIncident { get; set; }
    public string? ShiftInChargePersonName { get; set; }
    public string? Status { get; set; }
    public string? Remarks { get; set; }
    public DateTime? ModifiedDate { get; set; }
    public string? ModifiedBy { get; set; }
}

/// <summary>
/// Doctor/Attendant Master Entity
/// </summary>
public class DoctorAttendant
{
    public long DoctorAttendantId { get; set; }
    public string? Code { get; set; }
    public char Flag { get; set; } // D-DOCTOR, A-ATTENDANT
    public string? Name { get; set; }
    public string? Specialization { get; set; }
    public string? ContactNumber { get; set; }
    public bool IsActive { get; set; }
    public DateTime CreatedDate { get; set; }
    public DateTime? ModifiedDate { get; set; }
}
