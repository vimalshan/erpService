namespace CourseService.Domain.ValueObjects;

/// <summary>
/// Holds details for up to three trainers assigned to a course.
/// </summary>
public sealed record TrainerInfo
{
    public string? TrainerName1 { get; }
    public string? TrainerName2 { get; }
    public string? TrainerName3 { get; }
    public string? TrainerDesignation1 { get; }
    public string? TrainerDesignation2 { get; }
    public string? TrainerDesignation3 { get; }
    public string? TrainerContact1 { get; }
    public string? TrainerContact2 { get; }
    public string? TrainerContact3 { get; }
    public long? TrainerCode { get; }

    public TrainerInfo(
        string? trainerName1, string? trainerName2, string? trainerName3,
        string? trainerDesignation1, string? trainerDesignation2, string? trainerDesignation3,
        string? trainerContact1, string? trainerContact2, string? trainerContact3,
        long? trainerCode)
    {
        TrainerName1 = trainerName1;
        TrainerName2 = trainerName2;
        TrainerName3 = trainerName3;
        TrainerDesignation1 = trainerDesignation1;
        TrainerDesignation2 = trainerDesignation2;
        TrainerDesignation3 = trainerDesignation3;
        TrainerContact1 = trainerContact1;
        TrainerContact2 = trainerContact2;
        TrainerContact3 = trainerContact3;
        TrainerCode = trainerCode;
    }
}
