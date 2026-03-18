namespace RecruitmentService.Domain.Entities;

public class ApplicationQualification
{
    public decimal AppId { get; private set; }
    public decimal AppQualId { get; private set; }
    public decimal? QualCode { get; private set; }
    public string? QualDescription { get; private set; }
    public string? YearFrom { get; private set; }
    public string? YearTo { get; private set; }
    public string? InstitutionCode { get; private set; }
    public string? InstitutionDescription { get; private set; }
    public string? EducationType { get; private set; }
    public decimal? SpecializationCode { get; private set; }
    public string? SpecializationDescription { get; private set; }
    public string? Percentage { get; private set; }
    public decimal? DegreeCode { get; private set; }
    public string? DegreeDescription { get; private set; }
    public string? InstitutionOthers { get; private set; }

    private ApplicationQualification() { }

    public static ApplicationQualification Create(
        decimal appId, decimal qualId, decimal? qualCode, string? qualDesc,
        string? yearFrom, string? yearTo, string? instCode, string? instDesc,
        string? eduType, decimal? speCode, string? speDesc,
        string? percentage, decimal? degreeCode, string? degreeDesc,
        string? instOthers) =>
        new()
        {
            AppId = appId,
            AppQualId = qualId,
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
            InstitutionOthers = instOthers
        };
}
