namespace TimeAttendance.Domain.ValueObjects;

/// <summary>
/// Value object representing a period (year + month).
/// </summary>
public sealed class Period : IEquatable<Period>
{
    public int Year { get; }
    public int Month { get; }

    public Period(int year, int month)
    {
        if (year < 2000 || year > 2100) throw new ArgumentException("Year must be between 2000 and 2100.");
        if (month < 1 || month > 12) throw new ArgumentException("Month must be between 1 and 12.");
        Year = year;
        Month = month;
    }

    public string ToMonthString() => $"{Year}{Month:D2}";
    public DateOnly FirstDay() => new(Year, Month, 1);
    public DateOnly LastDay() => new(Year, Month, DateTime.DaysInMonth(Year, Month));

    public bool Equals(Period? other) => other is not null && Year == other.Year && Month == other.Month;
    public override bool Equals(object? obj) => obj is Period p && Equals(p);
    public override int GetHashCode() => HashCode.Combine(Year, Month);
    public override string ToString() => $"{Year}-{Month:D2}";
}
