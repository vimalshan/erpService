namespace PayTransactionalService.Domain.ValueObjects;

public sealed record MonthYear
{
    public int Year { get; }
    public int Month { get; }

    public MonthYear(int year, int month)
    {
        if (year < 2000 || year > 2100)
            throw new ArgumentOutOfRangeException(nameof(year));
        if (month < 1 || month > 12)
            throw new ArgumentOutOfRangeException(nameof(month));
        Year = year;
        Month = month;
    }

    public string Value => $"{Year:D4}-{Month:D2}";

    public static MonthYear FromString(string value)
    {
        var parts = value.Split('-');
        return new MonthYear(int.Parse(parts[0]), int.Parse(parts[1]));
    }

    public override string ToString() => Value;
}
