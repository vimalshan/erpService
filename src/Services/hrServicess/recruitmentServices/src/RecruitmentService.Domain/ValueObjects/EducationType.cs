namespace RecruitmentService.Domain.ValueObjects;

public enum EducationType
{
    FullTime,       // F
    PartTime,       // P
    Correspondence  // C
}

public static class EducationTypeExtensions
{
    public static string ToCode(this EducationType type) =>
        type switch
        {
            EducationType.FullTime => "F",
            EducationType.PartTime => "P",
            EducationType.Correspondence => "C",
            _ => throw new ArgumentOutOfRangeException(nameof(type))
        };

    public static EducationType? FromCode(string? code) =>
        code switch
        {
            "F" => EducationType.FullTime,
            "P" => EducationType.PartTime,
            "C" => EducationType.Correspondence,
            _ => null
        };
}
