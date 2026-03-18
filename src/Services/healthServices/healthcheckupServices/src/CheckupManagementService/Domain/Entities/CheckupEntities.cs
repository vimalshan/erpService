namespace CheckupManagementService.Domain.Entities;

using Shared.Core.Domain;

/// <summary>
/// Field Type Master - Field type definitions
/// </summary>
public class FieldTypeMaster
{
    public decimal FieldTypeCode { get; set; }
    public string? FieldTypeName { get; set; }
    public string? ControlSource { get; set; }
    
    // Navigation properties
    public virtual ICollection<CheckupOthers> CheckupOthers { get; set; } = new List<CheckupOthers>();
}

/// <summary>
/// Checkup Symptoms Master - Symptoms definitions
/// </summary>
public class CheckupSymptomMaster
{
    public decimal SymptomId { get; set; }
    public string? SymptomName { get; set; }
    public string? SymptomFlag { get; set; } // FH, PH, IM, CO
    
    // Navigation properties
    public virtual ICollection<CheckupPersonalFamilyHistory> PersonalFamilyHistories { get; set; } = new List<CheckupPersonalFamilyHistory>();
}

/// <summary>
/// Test Master - Available tests for checkups
/// </summary>
public class TestMaster
{
    public decimal TestCode { get; set; }
    public string TestName { get; set; } = string.Empty;
    public char? CheckboxFlag { get; set; }
    public DateTime? EffectiveDate { get; set; }
    public DateTime? CloseDate { get; set; }
    public char? CloseFlag { get; set; }
    public string? RangeValue { get; set; }
    public string? TestGroup { get; set; }
    
    // Additional properties for backward compatibility
    public string? TestId { get; set; }
    public string? TestCategory { get; set; } // Blood, X-Ray, ECG, etc.
    public string? NormalRange { get; set; }
    public string? Unit { get; set; }
    public decimal? Cost { get; set; }
    public bool IsActive { get; set; } = true;
    public DateTime CreatedOn { get; set; } = DateTime.UtcNow;
    public DateTime? UpdatedOn { get; set; }
    
    // Navigation properties
    public virtual ICollection<CheckupTest> CheckupTests { get; set; } = new List<CheckupTest>();
    public virtual ICollection<HealthMinMaxValue> HealthMinMaxValues { get; set; } = new List<HealthMinMaxValue>();
    public virtual ICollection<HealthEntryLov> HealthEntryLovs { get; set; } = new List<HealthEntryLov>();
}

/// <summary>
/// Checkup Master - Main checkup/examination records
/// </summary>
public class CheckupMaster
{
    public string CompanyCode { get; set; } = string.Empty;
    public decimal CheckupCode { get; set; }
    public string CheckupName { get; set; } = string.Empty;
    public DateTime EffectiveDate { get; set; }
    public string? CloseDate { get; set; }
    public char? Flag { get; set; } // B-Checkups, P-Pre-emp, C-Checkup Card
    
    // Additional properties for backward compatibility
    public string? CheckupMasterId { get; set; }
    public string? EmployeeNumber { get; set; }
    public string? CheckupType { get; set; }
    public DateTime CheckupDate { get; set; } = DateTime.UtcNow;
    public string? DoctorCode { get; set; }
    public string? DoctorRemarks { get; set; }
    public string? Status { get; set; } // Pending, Completed, Approved
    public string? ApprovedBy { get; set; }
    public DateTime? ApprovedDate { get; set; }
    public DateTime CreatedOn { get; set; } = DateTime.UtcNow;
    public DateTime? UpdatedOn { get; set; }
    public string? CreatedBy { get; set; }
    public string? UpdatedBy { get; set; }
    
    // Navigation properties
    public virtual ICollection<CheckupOthers> CheckupOthers { get; set; } = new List<CheckupOthers>();
    public virtual ICollection<CheckupTest> CheckupTests { get; set; } = new List<CheckupTest>();
    public virtual ICollection<HealthMain> HealthMains { get; set; } = new List<HealthMain>();
}

/// <summary>
/// Checkup Others - Additional checkup information/fields
/// </summary>
public class CheckupOthers
{
    public string CompanyCode { get; set; } = string.Empty;
    public decimal CheckupCode { get; set; }
    public decimal OtherSerialNumber { get; set; }
    public string? FieldLabel { get; set; }
    public char? MandatoryFlag { get; set; }
    public decimal? FieldTypeCode { get; set; }
    public DateTime EffectiveDate { get; set; }
    public DateTime? CloseDate { get; set; }
    public string? FieldTypeName { get; set; }
    public DateTime CreatedOn { get; set; } = DateTime.UtcNow;
    public DateTime? UpdatedOn { get; set; }
    
    // Backward compatibility properties
    public string? CheckupOthersId { get; set; }
    public string? CheckupMasterId { get; set; }
    public string? MedicineAllergy { get; set; }
    public string? FamilyHistory { get; set; }
    public string? PastSurgery { get; set; }
    public string? CurrentMedicines { get; set; }
    public string? LifestyleHabits { get; set; }
    public string? OtherComments { get; set; }
    
    // Navigation properties
    public virtual FieldTypeMaster? FieldType { get; set; }
    public virtual ICollection<CheckupOthersListOfValues> ListOfValues { get; set; } = new List<CheckupOthersListOfValues>();
}

/// <summary>
/// Checkup Others List of Values - LOV for other fields
/// </summary>
public class CheckupOthersListOfValues
{
    public decimal ListOfValueSerialNumber { get; set; }
    public string? CompanyCode { get; set; }
    public decimal? CheckupCode { get; set; }
    public decimal? OtherSerialNumber { get; set; }
    public string? ListOfValueDescription { get; set; }
    
    // Navigation property
    public virtual CheckupOthers? CheckupOther { get; set; }
}

/// <summary>
/// Checkup Test - Tests mapped to checkups
/// </summary>
public class CheckupTest
{
    public decimal SerialNumber { get; set; }
    public decimal? CheckupCode { get; set; }
    public string? CompanyCode { get; set; }
    public decimal? TestCode { get; set; }
    public decimal? OrderNumber { get; set; }
    public char? CheckboxFlag { get; set; }
    public DateTime? EffectiveDate { get; set; }
    public DateTime? CloseDate { get; set; }
    public char? CloseFlag { get; set; }
    
    // Navigation properties
    public virtual CheckupMaster? CheckupMaster { get; set; }
    public virtual TestMaster? TestMaster { get; set; }
}

/// <summary>
/// Health Counter - Counter management
/// </summary>
public class HealthCounter
{
    public string CompanyCode { get; set; } = string.Empty;
    public string CounterCode { get; set; } = string.Empty;
    public long CounterValue { get; set; }
}

/// <summary>
/// Health Min/Max Values - Test reference ranges
/// </summary>
public class HealthMinMaxValue
{
    public decimal TestCode { get; set; }
    public string? TypeCode { get; set; } // NS, ND, SV, LO
    public string? UnitCode { get; set; }
    public decimal? SingleValue { get; set; }
    public decimal? MinValue { get; set; }
    public decimal? MaxValue { get; set; }
    public string? MinText { get; set; }
    public string? MaxText { get; set; }
    public string? LovText { get; set; }
    
    // Navigation property
    public virtual TestMaster? TestMaster { get; set; }
}

/// <summary>
/// Health Entry List of Values
/// </summary>
public class HealthEntryLov
{
    public decimal TestCode { get; set; }
    public string? ListOfValueText { get; set; }
    
    // Navigation property
    public virtual TestMaster? TestMaster { get; set; }
}

/// <summary>
/// Health Main - Health examination details
/// </summary>
public class HealthMain
{
    public string EmployeeNumber { get; set; } = string.Empty;
    public string CompanyCode { get; set; } = string.Empty;
    public string? CheckupDate { get; set; }
    public int HealthNumber { get; set; }
    public string EntryEmployeeNumber { get; set; } = string.Empty;
    public decimal CheckupCode { get; set; }
    public string? TextField2 { get; set; }
    public string? TextField3 { get; set; }
    public string? TextField4 { get; set; }
    public string? TextField5 { get; set; }
    
    // Additional backward compatibility properties
    public string? HealthId { get; set; }
    public string? CheckupMasterId { get; set; }
    public decimal? Height { get; set; }
    public decimal? Weight { get; set; }
    public decimal? BMI { get; set; }
    public decimal? BloodPressure { get; set; }
    public int? HeartRate { get; set; }
    public string? BloodGroup { get; set; }
    public string? EyeVision { get; set; }
    public string? ColorBlindness { get; set; }
    public string? Hearing { get; set; }
    public string? LungsXRay { get; set; }
    public string? ECG { get; set; }
    public string? OverallFitness { get; set; } // Fit, Unfit, Conditional
    public string? MedicalClearance { get; set; }
    public string? Recommendations { get; set; }
    public DateTime CreatedOn { get; set; } = DateTime.UtcNow;
    public DateTime? UpdatedOn { get; set; }
    
    // Navigation properties
    public virtual CheckupMaster? CheckupMaster { get; set; }
    public virtual ICollection<HealthSub> HealthSubs { get; set; } = new List<HealthSub>();
}

/// <summary>
/// Health Sub - Individual test results
/// </summary>
public class HealthSub
{
    public int HealthNumber { get; set; }
    public string? TestCode { get; set; }
    public string? TestType { get; set; }
    public string? TestValue { get; set; }
    public string EmployeeNumber { get; set; } = string.Empty;
    public string? TestRemarks { get; set; }
    public DateTime? TestDate { get; set; }
    public char? ValidationFlag { get; set; }
    public string? TextField2 { get; set; }
    public string? TextField3 { get; set; }
    public string? TextField4 { get; set; }
    public string? TextField5 { get; set; }
    public string? DoctorRemarks { get; set; }
    public DateTime CreatedOn { get; set; } = DateTime.UtcNow;
    
    // Backward compatibility properties
    public string? Remarks { get; set; }
    public string? HealthSubId { get; set; }
    public string? HealthId { get; set; }
    public string? TestName { get; set; }
    public string? Result { get; set; } // Normal, Abnormal
    public string? NormalRange { get; set; }
    
    // Navigation property
    public virtual HealthMain? HealthMain { get; set; }
}

/// <summary>
/// Health Dynamic Details - Dynamic field values
/// </summary>
public class HealthDynamicDetail
{
    public decimal? HealthNumber { get; set; }
    public decimal? CheckupCode { get; set; }
    public string? CompanyCode { get; set; }
    public decimal? ControlSourceId { get; set; }
    public string? DynamicValue { get; set; }
    public decimal? EmployeeNumber { get; set; }
    public DateTime? SystemDate { get; set; }
}

/// <summary>
/// Pre-Employment Checkup Main
/// </summary>
public class PreEmploymentCheckupMain
{
    public decimal? EmployeeNumber { get; set; }
    public string? CompanyCode { get; set; }
    public decimal? HealthNumber { get; set; }
    public string? PhysicalHandicapDescription { get; set; }
    public string? ProposedDesignation { get; set; }
    public string? IdentificationMarks { get; set; }
    public string? FinalRemarks { get; set; }
    public string? FitPhysical { get; set; }
    public string? FitFinal { get; set; }
    public DateTime? CheckupDate { get; set; }
}

/// <summary>
/// Checkup Personal & Family History
/// </summary>
public class CheckupPersonalFamilyHistory
{
    public decimal? HealthNumber { get; set; }
    public decimal? EmployeeNumber { get; set; }
    public decimal? SymptomId { get; set; }
    public char? YesNoFlag { get; set; }
    public DateTime? ImmunizationDate { get; set; }
    public string? TestValue { get; set; }
    
    // Navigation properties
    public virtual CheckupSymptomMaster? Symptom { get; set; }
}

/// <summary>
/// Health Check Card - Checkup card/certificate
/// </summary>
public class HealthCheckCard
{
    public decimal? HealthNumber { get; set; }
    public decimal? EmployeeNumber { get; set; }
    public DateTime? EmployeeDate { get; set; }
    public string? CompanyCode { get; set; }
    public string? PersonalDetails { get; set; }
    public string? ScreeningDetails { get; set; }
    public string? AdviceRemark1 { get; set; }
    public DateTime? DoctorDate1 { get; set; }
    public string? AdviceFollowup1 { get; set; }
    public string? AdviceRemark2 { get; set; }
    public DateTime? DoctorDate2 { get; set; }
    public string? AdviceFollowup2 { get; set; }
    
    // Backward compatibility properties
    public string? CardNumber { get; set; }
    public string? CheckupMasterId { get; set; }
    public DateTime IssueDate { get; set; } = DateTime.UtcNow;
    public DateTime? ExpiryDate { get; set; }
    public string? CardStatus { get; set; } // Valid, Expired, Suspended
    public string? IssuedBy { get; set; }
    public DateTime CreatedOn { get; set; } = DateTime.UtcNow;
}

/// <summary>
/// Health Check Card Sub - Checkup card symptoms details
/// </summary>
public class HealthCheckCardSub
{
    public decimal? HealthNumber { get; set; }
    public decimal? SymptomId { get; set; }
    public string? FlagYesNo { get; set; }
    public string? SymptomValue { get; set; }
    public decimal? EmployeeNumber { get; set; }
}

/// <summary>
/// Checkup Test Link - Legacy entity mapping for CheckupTest compatibility
/// </summary>
public class CheckupTestLink
{
    public string LinkId { get; set; } = string.Empty;
    public string CheckupMasterId { get; set; } = string.Empty;
    public string TestId { get; set; } = string.Empty;
    public bool IsRequired { get; set; } = true;
    public bool IsCompleted { get; set; } = false;
    public DateTime CreatedOn { get; set; } = DateTime.UtcNow;
    public DateTime? CompletedOn { get; set; }

    // Navigation properties
    public virtual CheckupMaster? CheckupMaster { get; set; }
    public virtual TestMaster? TestMaster { get; set; }
}
