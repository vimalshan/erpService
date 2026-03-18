namespace DeductionService.Domain.ValueObjects;

/// <summary>
/// Value object representing a payroll period as YYYY-MM.
/// </summary>
public sealed record MonthYear
{
    public int Year { get; }
    public int Month { get; }

    private MonthYear(int year, int month)
    {
        Year = year;
        Month = month;
    }

    public static MonthYear Create(int year, int month)
    {
        if (year < 2000 || year > 2100)
            throw new ArgumentException("Year must be between 2000 and 2100.", nameof(year));
        if (month < 1 || month > 12)
            throw new ArgumentException("Month must be between 1 and 12.", nameof(month));

        return new MonthYear(year, month);
    }

    public static MonthYear Parse(string value)
    {
        if (string.IsNullOrWhiteSpace(value) || value.Length != 7 || value[4] != '-')
            throw new FormatException($"Invalid MonthYear format '{value}'. Expected YYYY-MM.");

        var year = int.Parse(value[..4]);
        var month = int.Parse(value[5..]);
        return Create(year, month);
    }

    public override string ToString() => $"{Year:D4}-{Month:D2}";
}
