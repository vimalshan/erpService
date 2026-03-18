namespace TimeAttendance.Domain.ValueObjects;

/// <summary>
/// Value object representing a man-days metric.
/// </summary>
public sealed class ManDays : IEquatable<ManDays>
{
    public long Total { get; }
    public long Absent { get; }
    public long Present => Total - Absent;
    public decimal AbsenteeismRate => Total == 0 ? 0 : Math.Round((decimal)Absent / Total * 100, 2);

    public ManDays(long total, long absent)
    {
        if (total < 0) throw new ArgumentException("Total man days cannot be negative.");
        if (absent < 0) throw new ArgumentException("Absent man days cannot be negative.");
        if (absent > total) throw new ArgumentException("Absent cannot exceed total man days.");
        Total = total;
        Absent = absent;
    }

    public bool Equals(ManDays? other) => other is not null && Total == other.Total && Absent == other.Absent;
    public override bool Equals(object? obj) => obj is ManDays m && Equals(m);
    public override int GetHashCode() => HashCode.Combine(Total, Absent);
    public override string ToString() => $"Total: {Total}, Absent: {Absent}, Rate: {AbsenteeismRate}%";
}
