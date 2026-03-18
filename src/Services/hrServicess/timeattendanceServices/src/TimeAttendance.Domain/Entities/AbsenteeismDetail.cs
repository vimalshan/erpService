using TimeAttendance.Domain.Common;

namespace TimeAttendance.Domain.Entities;

/// <summary>
/// Aggregate root for absenteeism detail records.
/// Maps to ABSENTEEISM_DET table.
/// </summary>
public class AbsenteeismDetail : AuditableEntity
{
    public long Id { get; private set; }
    public long UnitId { get; private set; }
    public int Year { get; private set; }
    public int Month { get; private set; }
    public long TotalManDays { get; private set; }
    public long AbsentManDays { get; private set; }
    public string GradeCategory { get; private set; } = string.Empty;
    public long FunctionId { get; private set; }
    public long AgeId { get; private set; }
    public long ExperienceId { get; private set; }
    public char Gender { get; private set; }
    public long InternalExperienceId { get; private set; }
    public long TotalExperienceId { get; private set; }

    // Computed property
    public decimal AbsenteeismRate =>
        TotalManDays == 0 ? 0 : Math.Round((decimal)AbsentManDays / TotalManDays * 100, 2);

    private AbsenteeismDetail() { } // EF constructor

    public static AbsenteeismDetail Create(
        long unitId, int year, int month,
        long totalManDays, long absentManDays,
        string gradeCategory, long functionId,
        long ageId, long experienceId, char gender,
        long internalExperienceId, long totalExperienceId)
    {
        if (totalManDays < 0) throw new ArgumentException("Total man days cannot be negative.");
        if (absentManDays < 0) throw new ArgumentException("Absent man days cannot be negative.");
        if (absentManDays > totalManDays) throw new ArgumentException("Absent man days cannot exceed total man days.");
        if (year < 2000 || year > 2100) throw new ArgumentException("Invalid year value.");
        if (month < 1 || month > 12) throw new ArgumentException("Month must be between 1 and 12.");

        var entity = new AbsenteeismDetail
        {
            UnitId = unitId,
            Year = year,
            Month = month,
            TotalManDays = totalManDays,
            AbsentManDays = absentManDays,
            GradeCategory = gradeCategory,
            FunctionId = functionId,
            AgeId = ageId,
            ExperienceId = experienceId,
            Gender = gender,
            InternalExperienceId = internalExperienceId,
            TotalExperienceId = totalExperienceId
        };

        entity.AddDomainEvent(new Events.AbsenteeismDetailCreatedEvent(entity.Id, unitId, year, month));
        return entity;
    }

    public void Update(long totalManDays, long absentManDays, string gradeCategory)
    {
        if (totalManDays < 0) throw new ArgumentException("Total man days cannot be negative.");
        if (absentManDays > totalManDays) throw new ArgumentException("Absent man days cannot exceed total man days.");

        TotalManDays = totalManDays;
        AbsentManDays = absentManDays;
        GradeCategory = gradeCategory;
        LastModifiedAt = DateTime.UtcNow;

        AddDomainEvent(new Events.AbsenteeismDetailUpdatedEvent(Id, UnitId));
    }
}
