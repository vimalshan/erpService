namespace CompensationBenefits.Application.DTOs;

public class SalaryDto
{
    public long SalaryId { get; set; }
    public string SalaryType { get; set; } = default!;
    public decimal SalaryCTC { get; set; }
    public long SalaryStructureId { get; set; }
    public long SalaryFooterId { get; set; }
    public long? SalaryCopyEmpSysId { get; set; }
    public long SalaryCreatedBy { get; set; }
    public DateTime SalaryCreatedOn { get; set; }
    public long? SalaryCancelledBy { get; set; }
    public DateTime? SalaryCancelledOn { get; set; }
    public List<SalaryDetailDto> Details { get; set; } = [];
}

public class SalaryDetailDto
{
    public long SalDetId { get; set; }
    public long SalDetSalaryId { get; set; }
    public decimal SalDetSrl { get; set; }
    public string? SalDetAnnGroup { get; set; }
    public long SalDetEdId { get; set; }
    public string SalDetCategory { get; set; } = default!;
    public string SalDetEdName { get; set; } = default!;
    public decimal SalDetEdAmt { get; set; }
    public string SalDetFrequency { get; set; } = default!;
}

public class SalaryStructureDto
{
    public long StructureId { get; set; }
    public long StructureUnitId { get; set; }
    public string StructureName { get; set; } = default!;
    public string StructureGradeCategory { get; set; } = default!;
    public long StructureGradeId { get; set; }
    public string StructureType { get; set; } = default!;
    public decimal StructureCtcMin { get; set; }
    public decimal StructureCtcMax { get; set; }
    public long StructureFooterId { get; set; }
    public DateTime? StructureClsDate { get; set; }
    public List<SalaryStructureDetailDto> Details { get; set; } = [];
}

public class SalaryStructureDetailDto
{
    public long StructDetId { get; set; }
    public long StructDetStructureId { get; set; }
    public long StructDetEdId { get; set; }
    public string StructDetCategory { get; set; } = default!;
    public decimal StructDetEdAmt { get; set; }
    public decimal StructDetMinValue { get; set; }
    public decimal? StructDetMaxValue { get; set; }
    public string StructDetFrequency { get; set; } = default!;
}

public class MediclaimDto
{
    public long MediclaimId { get; set; }
    public string? MediclaimRefName { get; set; }
    public DateTime? MediclaimStartDate { get; set; }
    public DateTime? MediclaimCloseDate { get; set; }
    public string? MediclaimType { get; set; }
    public string? MediclaimPaidBy { get; set; }
    public decimal? MediclaimCompPayLimit { get; set; }
    public List<MediclaimYearlyPremiumDto> YearlyPremiums { get; set; } = [];
}

public class MediclaimYearlyPremiumDto
{
    public long MedYpYearlyPremId { get; set; }
    public decimal MedYpSumAssured { get; set; }
    public decimal MedYpPremiumAmnt { get; set; }
    public string MedYpType { get; set; } = default!;
}

public class MobileConnectionDto
{
    public long ConnId { get; set; }
    public long ConnEmpSysId { get; set; }
    public DateTime ConnEffDate { get; set; }
    public DateTime? ConnClsDate { get; set; }
    public string ConnType { get; set; } = default!;
    public long ConnPhoneNo { get; set; }
    public string? ConnRemarks { get; set; }
}

public class RetiralRangeMasterDto
{
    public long RrMastId { get; set; }
    public long RrMastUnitId { get; set; }
    public decimal RrMastFromYear { get; set; }
    public decimal RrMastToYear { get; set; }
    public decimal RrMastPercentage { get; set; }
}
