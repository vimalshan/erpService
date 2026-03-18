namespace RecruitmentService.Domain.Entities;

public class ProspectQualification
{
    public decimal EmpSysId { get; private set; }
    public decimal QualId { get; private set; }
    public decimal QualCode { get; private set; }
    public string? QualDescription { get; private set; }
    public string? YearFrom { get; private set; }
    public string? YearTo { get; private set; }
    public decimal? InstitutionCode { get; private set; }
    public string? InstitutionDescription { get; private set; }
    public string? EducationType { get; private set; }
    public decimal? SpecializationCode { get; private set; }
    public string? SpecializationDescription { get; private set; }
    public string? Percentage { get; private set; }
    public decimal? DegreeCode { get; private set; }
    public string? DegreeDescription { get; private set; }
    public decimal? UpdatedBy { get; private set; }
    public DateTime? UpdatedOn { get; private set; }

    private ProspectQualification() { }

    public static ProspectQualification Create(
        decimal empSysId, decimal qualId, decimal qualCode, string? qualDesc,
        string? yearFrom, string? yearTo, decimal? instCode, string? instDesc,
        string? eduType, decimal? speCode, string? speDesc,
        string? percentage, decimal? degreeCode, string? degreeDesc,
        decimal? updatedBy) =>
        new()
        {
            EmpSysId = empSysId,
            QualId = qualId,
            QualCode = qualCode,
            QualDescription = qualDesc,
            YearFrom = yearFrom,
            YearTo = yearTo,
            InstitutionCode = instCode,
            InstitutionDescription = instDesc,
            EducationType = eduType,
            SpecializationCode = speCode,
            SpecializationDescription = speDesc,
            Percentage = percentage,
            DegreeCode = degreeCode,
            DegreeDescription = degreeDesc,
            UpdatedBy = updatedBy,
            UpdatedOn = DateTime.UtcNow
        };
}
