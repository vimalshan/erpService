namespace EmployeeService.Domain.ValueObjects;

/// <summary>Attendance flag value object — 'P' present, 'A' absent, 'L' leave, etc.</summary>
public sealed class AttendanceFlag : IEquatable<AttendanceFlag>
{
    private static readonly HashSet<char> _validFlags = ['P', 'A', 'L', 'H', 'W', 'X'];

    public char Value { get; }

    private AttendanceFlag(char value) => Value = value;

    public static AttendanceFlag Of(char value)
    {
        if (!_validFlags.Contains(char.ToUpperInvariant(value)))
            throw new ArgumentException($"Invalid attendance flag '{value}'. Valid: P,A,L,H,W,X", nameof(value));
        return new AttendanceFlag(char.ToUpperInvariant(value));
    }

    public static AttendanceFlag Of(string value)
    {
        if (string.IsNullOrWhiteSpace(value) || value.Length != 1)
            throw new ArgumentException("Attendance flag must be a single character.", nameof(value));
        return Of(value[0]);
    }

    public bool Equals(AttendanceFlag? other) => other is not null && Value == other.Value;
    public override bool Equals(object? obj) => obj is AttendanceFlag af && Equals(af);
    public override int GetHashCode() => Value.GetHashCode();
    public override string ToString() => Value.ToString();
}
