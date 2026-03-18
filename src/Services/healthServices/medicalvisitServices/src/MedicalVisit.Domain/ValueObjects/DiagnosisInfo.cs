using MedicalVisit.Domain.Common;

namespace MedicalVisit.Domain.ValueObjects;

public class DiagnosisInfo : ValueObject
{
    public string PatientDiagnosis { get; private set; }
    public string TreatmentRemarks { get; private set; }
    public string? TestAdvice { get; private set; }
    public string? DoctorRemarks { get; private set; }
    public string? DiagnosisCategory { get; private set; }
    public long? DiagnosisSubCategory { get; private set; }

    private DiagnosisInfo(
        string patientDiagnosis,
        string treatmentRemarks,
        string? testAdvice,
        string? doctorRemarks,
        string? diagnosisCategory,
        long? diagnosisSubCategory)
    {
        PatientDiagnosis = patientDiagnosis;
        TreatmentRemarks = treatmentRemarks;
        TestAdvice = testAdvice;
        DoctorRemarks = doctorRemarks;
        DiagnosisCategory = diagnosisCategory;
        DiagnosisSubCategory = diagnosisSubCategory;
    }

    public static DiagnosisInfo Create(
        string patientDiagnosis,
        string treatmentRemarks,
        string? testAdvice = null,
        string? doctorRemarks = null,
        string? diagnosisCategory = null,
        long? diagnosisSubCategory = null)
    {
        if (string.IsNullOrWhiteSpace(patientDiagnosis))
            throw new ArgumentException("Patient diagnosis is required", nameof(patientDiagnosis));

        if (string.IsNullOrWhiteSpace(treatmentRemarks))
            throw new ArgumentException("Treatment remarks are required", nameof(treatmentRemarks));

        if (patientDiagnosis.Length > 200)
            throw new ArgumentException("Patient diagnosis cannot exceed 200 characters", nameof(patientDiagnosis));

        if (treatmentRemarks.Length > 200)
            throw new ArgumentException("Treatment remarks cannot exceed 200 characters", nameof(treatmentRemarks));

        if (doctorRemarks?.Length > 1000)
            throw new ArgumentException("Doctor remarks cannot exceed 1000 characters", nameof(doctorRemarks));

        return new DiagnosisInfo(
            patientDiagnosis,
            treatmentRemarks,
            testAdvice,
            doctorRemarks,
            diagnosisCategory,
            diagnosisSubCategory);
    }

    protected override IEnumerable<object> GetEqualityComponents()
    {
        yield return PatientDiagnosis;
        yield return TreatmentRemarks;
        yield return TestAdvice ?? string.Empty;
        yield return DoctorRemarks ?? string.Empty;
        yield return DiagnosisCategory ?? string.Empty;
        yield return DiagnosisSubCategory ?? 0;
    }
}
