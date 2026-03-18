using System.Text.RegularExpressions;

namespace StipendService.Domain.ValueObjects;

public sealed class MonthYear
{
    public string Value { get; }
    public int Year { get; }
    public int Month { get; }

    private static readonly Regex Pattern = new(@"^\d{4}-(0[1-9]|1[0-2])$", RegexOptions.Compiled);

    private MonthYear(string value)
    {
        Value = value;
        var parts = value.Split('-');
        Year = int.Parse(parts[0]);
        Month = int.Parse(parts[1]);
    }

    public static MonthYear Create(string value)
    {
        if (string.IsNullOrWhiteSpace(value) || !Pattern.IsMatch(value))
            throw new ArgumentException("MonthYear must be in format YYYY-MM.", nameof(value));
        return new MonthYear(value);
    }

    public static MonthYear FromDate(DateTime date) =>
        Create($"{date.Year:D4}-{date.Month:D2}");

    public override string ToString() => Value;
    public override bool Equals(object? obj) => obj is MonthYear other && Value == other.Value;
    public override int GetHashCode() => Value.GetHashCode();
}
