using EmployeeManagement.Domain.Common;

namespace EmployeeManagement.Domain.Entities;

public sealed class EmployeeQualification : BaseEntity
{
    public long QualificationId { get; private set; }
    public long EmployeeId { get; private set; }
    public long? QualCode { get; private set; }
    public string? QualDescription { get; private set; }
    public string? YearFrom { get; private set; }  // MMYYYY
    public string? YearTo { get; private set; }    // MMYYYY
    public long? InstitutionCode { get; private set; }
    public string? InstitutionDesc { get; private set; }
    public char? EducationType { get; private set; }  // F/P/C
    public long? SpecializationCode { get; private set; }
    public string? SpecializationDesc { get; private set; }
    public string? Percentage { get; private set; }
    public long? DegreeCode { get; private set; }
    public string? DegreeDesc { get; private set; }
    public long? UpdatedBy { get; private set; }
    public DateTime? UpdatedOn { get; private set; }

    private EmployeeQualification() { }

    public static EmployeeQualification Create(long id, long employeeId, long? qualCode,
        string? qualDesc, string? yearFrom, string? yearTo, string? institutionDesc,
        char? eduType, string? percentage, string? degreeDesc, long updatedBy)
    {
        return new EmployeeQualification
        {
            QualificationId = id, EmployeeId = employeeId, QualCode = qualCode,
            QualDescription = qualDesc, YearFrom = yearFrom, YearTo = yearTo,
            InstitutionDesc = institutionDesc, EducationType = eduType,
            Percentage = percentage, DegreeDesc = degreeDesc,
            UpdatedBy = updatedBy, UpdatedOn = DateTime.UtcNow
        };
    }
}
